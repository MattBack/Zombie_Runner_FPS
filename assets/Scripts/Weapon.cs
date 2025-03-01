using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FireMode
{
    Single,
    Continuous,
    Shotgun,
    Minigun
}

public class Weapon : MonoBehaviour
{
    Transform cam;
    // impact references
    //public ImpactInfo[] ImpactElemets = new ImpactInfo[0];
    ImpactElementHandler ImpactElementHandler;
    private Animator weaponAnimator;
    [Space]
    public float BulletDistance = 100; // TODO: remove
    public GameObject ImpactEffect;
    // impact references end

    [SerializeField] String weaponName;
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 30f;
    //[SerializeField] GameObject muzzleFlash;
    //[SerializeField] ParticleSystem muzzleFlash; // TODO: remove if not needed
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject hitEffectBlood;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] TextMeshProUGUI ammoText;
    
    [Header("Gun SFX")]
    [SerializeField] float reloadSfxDelay = 0.5f;
    [SerializeField] float shellsFallingDelay = 0.3f;
    [SerializeField] AudioSource pistolSfx;
    public AudioClip clip;
    public AudioClip afterShotSfx;
    public AudioClip thirdGunSfx;
    [SerializeField] AudioClip weaponClipEmptySfx;

    AudioManager audioManager;

    [SerializeField] GameObject currentWeaponMuzzleRef;
    [SerializeField] GameObject animatorWeaponRef;
    public GameObject ref_rightHand;

    public FireMode fireMode;

    [SerializeField] bool shotgun = false;
    [SerializeField] int bulletsPerShot = 6;

    [SerializeField] float inaccuracyDistance = 5f;

    // for minigun animation logic
    private bool isShooting = false;
    private bool minigunCanStartShooting = false;
    private bool minigunIsFiring = false;

    [Header("Blood Decal")]
    // BLOOD ********************************************
    // public bool InfiniteDecal;
    // public Light DirLight;
    public GameObject BloodAttach;
    //public GameObject[] BloodFX;

    BloodHandler BloodHandler; // references to blood decals
    //BLOOD *******************************************

    bool canShoot = true;

    [Header("Reload info")]
    [SerializeField] AudioClip reloadingWeaponSound;
    [SerializeField] int clipSize = 7;
    int currentClipAmmo;

    [Header("Camera shake config")]
    [SerializeField] float shakeDuration = 0.1f;
    [SerializeField] float shakeMagnitude = 0.2f;

    
    private Melee melee;
    [Header("Melee config")]
    [SerializeField] float meleeAttackDamage = 10f;
    [SerializeField] float meleeAttackDuration = 1f;
    public BoxCollider meleeCollider;

    private bool isAttacking = false;

    private void Awake()
    {
        cam = FPCamera.transform;
        audioManager = FindObjectOfType<AudioManager>();
        BloodHandler = FindObjectOfType<BloodHandler>();
        //melee = FindObjectOfType<Melee>();

        ImpactElementHandler = FindObjectOfType<ImpactElementHandler>();
        weaponAnimator = animatorWeaponRef.GetComponent<Animator>();
        SetIdleAnimation();
    }

    private void OnEnable()
    {
        canShoot = true;
        currentClipAmmo = Mathf.Min(clipSize, ammoSlot.GetCurrentAmmo(ammoType));
    }

    // for melee attack
    void OnTriggerEnter(Collider other)
    {
        if (meleeCollider.enabled && other.CompareTag("Enemy"))
        {
            ProcessMeleeHit(other);
        }
    }

    void ProcessMeleeHit(Collider other)
    {
        try
        {
            EnemyHealth target = other.GetComponent<EnemyHealth>();
            if (target == null) return;

            PlayImpactSfx();

            // BLOOD START ********************************************************************
            Vector3 hitPoint = other.ClosestPoint(transform.position); // Get the impact point
            Vector3 hitNormal = (hitPoint - transform.position).normalized; // Approximate hit direction

            float angle = Mathf.Atan2(hitNormal.x, hitNormal.z) * Mathf.Rad2Deg + 180;
            if (effectIdx == BloodHandler.BloodFX.Length) effectIdx = 0;

            var instance = Instantiate(BloodHandler.BloodFX[effectIdx], hitPoint, Quaternion.Euler(0, angle + 90, 0));
            effectIdx++;

            var settings = instance.GetComponent<BFX_BloodSettings>();
            settings.GroundHeight = target.transform.position.y; // Set ground height for blood effects

            var nearestBone = GetNearestObject(other.transform.root, hitPoint);
            if (nearestBone != null)
            {
                var attachBloodInstance = Instantiate(BloodAttach);
                var bloodT = attachBloodInstance.transform;
                bloodT.position = hitPoint;
                bloodT.localRotation = Quaternion.identity;
                bloodT.localScale = Vector3.one * UnityEngine.Random.Range(0.75f, 1.2f);
                bloodT.LookAt(hitPoint + hitNormal, hitNormal);
                bloodT.Rotate(90, 0, 0);
                bloodT.transform.parent = nearestBone;
            }
            // BLOOD END ********************************************************************

            target.TakeDamage(meleeAttackDamage);
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
    }

    IEnumerator PerformMeleeAttack()
    {
        isAttacking = true;
        weaponAnimator.Play("Melee_Attack");
        meleeCollider.enabled = true;
        yield return new WaitForSeconds(meleeAttackDuration);

        meleeCollider.enabled = false;

        isAttacking = false;
    }

    // Input.GetButton - for rapid fire.
    void Update()
    {
        DisplayAmmo();

        if (Input.GetKeyDown(KeyCode.V) && !isAttacking)
        {
            StartCoroutine(PerformMeleeAttack());
            // trigger sound;
        }


        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (fireMode == FireMode.Continuous)
        {
            if (Input.GetButton("Fire1") && canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
        else if (fireMode == FireMode.Single)
        {
            if (Input.GetButtonDown("Fire1") && canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
        else if (fireMode == FireMode.Shotgun)
        {
            if (Input.GetButtonDown("Fire1") && canShoot)
            {
                shotgun = true;
                StartCoroutine(Shoot());
            }
        }
        else if (fireMode == FireMode.Minigun)
        {
            if (Input.GetButton("Fire1") && canShoot && currentClipAmmo > 0)
            {
                Debug.Log("Fire1 pressed");

                if (!isShooting)
                {
                    isShooting = true;
                    minigunCanStartShooting = false;
                    weaponAnimator.Play("AN_StartShoot");
                    StartCoroutine(WaitForStartShootAnimation());
                }

                if (minigunCanStartShooting)
                {
                    if (!minigunIsFiring)
                    {
                        minigunIsFiring = true;
                        weaponAnimator.Play("AN_Shooting");
                        StartCoroutine(ShootContinuously());
                    }
                }
            }
            else if (isShooting)
            {
                isShooting = false;
                minigunIsFiring = false;
                weaponAnimator.Play("AN_EndShoot");
                StartCoroutine(WaitForEndShootAnimation());
            }
            else if (Input.GetButton("Fire1") && canShoot && currentClipAmmo <= 0)
            {
                // If not ammo in clip then don't start the routines.
                // Play SFX
                PlayEmptyClip();
            }
        }
    }

    private void PlayEmptyClip()
    {
        if (weaponClipEmptySfx != null)
        {
            if (!pistolSfx.isPlaying) // Ensure no overlap
            {
                Debug.Log("Play minigun empty Sound Clip");
                pistolSfx.PlayOneShot(weaponClipEmptySfx);
            }
        }
        else
        {
            Debug.LogWarning("No empty sound clip assigned for this weapon!");
        }
    }

    // minigun animation logic start

    IEnumerator WaitForStartShootAnimation()
    {
        float animLength = weaponAnimator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log("AN_StartShoot length: " + animLength);
        // Wait until the "AN_StartShoot" animation has finished
        yield return new WaitForSeconds(animLength);

        minigunCanStartShooting = true;
        Debug.Log("AN_StartShoot completed, minigunCanStartShooting = " + minigunCanStartShooting);
    }

    IEnumerator WaitForEndShootAnimation()
    {
        float animLength = weaponAnimator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log("AN_EndShoot length: " + animLength);
        // Wait until the "AN_EndShoot" animation has finished
        yield return new WaitForSeconds(weaponAnimator.GetCurrentAnimatorStateInfo(0).length);
        
        // After the barrel slows down, switch to idle
        SetIdleAnimation();
    }

    // Continuous shooting coroutine
    IEnumerator ShootContinuously()
    {
        while (Input.GetButton("Fire1") && currentClipAmmo > 0)
        {
            // Ensure firing state is active
            if (!minigunIsFiring)
            {
                minigunIsFiring = true;
                weaponAnimator.Play("AN_Shooting");
            }

            PlayPistolSFX();
            PlayImpactSfx();
            ProcessRaycast();  // Simulate bullet hit
            currentClipAmmo--;  // Decrease ammo
            shakeOnShoot();  // Add camera shake effect

            yield return new WaitForSeconds(timeBetweenShots);  // Delay between shots
        }

        // Stop firing when ammo is depleted or Fire1 is released
        minigunIsFiring = false;

        if (currentClipAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            weaponAnimator.Play("AN_EndShoot");
            StartCoroutine(WaitForEndShootAnimation());
        }
        else if (!Input.GetButton("Fire1"))
        {
            Debug.Log("Fire1 released!");
            weaponAnimator.Play("AN_EndShoot");
            StartCoroutine(WaitForEndShootAnimation());
        }
    }
    // minigun animation logic end

    // added to ImpactElementHandler 
    // TODO: muzzle flash   *****************************************************
    [System.Serializable]
    public class ImpactInfo
    {
        public MaterialType.MaterialTypeEnum MaterialType;
        public GameObject ImpactEffect;
    }

    GameObject GetImpactEffect(GameObject impactedGameObject)
    {
        var materialType = impactedGameObject.GetComponent<MaterialType>();
        if (materialType == null)
            return null;
        foreach (var impactInfo in ImpactElementHandler.ImpactElemets)
        {
            if (impactInfo.MaterialType == materialType.TypeOfMaterial)
                return impactInfo.ImpactEffect;
        }
        return null;
    }
    // TODO: Muzzle flash end *******************************************************


    private void DisplayAmmo()
    {
        int ammoInReserve = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = $"{currentClipAmmo}/{ammoInReserve}";
    }


    IEnumerator Shoot()
    {
        canShoot = false;

        if (currentClipAmmo > 0)
        {
            if (shotgun)
            {
                PlayPistolSFX();
                PlayImpactSfx();
                currentClipAmmo--;
                StartCoroutine(StartRecoil());
                shakeOnShoot();
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    ProcessRaycast();
                }
            }
            else
            {
                // PlayMuzzleFlash(); // TODO: maybe remove if not needed
                PlayPistolSFX();
                PlayImpactSfx();
                // Play impact sound depending on surface type
                ProcessRaycast();
                currentClipAmmo--;
                StartCoroutine(StartRecoil());
                shakeOnShoot();
            }

        }
        else
        {
            if (weaponClipEmptySfx != null)
            {
                pistolSfx.PlayOneShot(weaponClipEmptySfx);
            }
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private bool isShaking = false; // Track if the camera is currently shaking

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        if (isShaking) yield break; // Prevent overlapping shakes
        isShaking = true;

        Transform shakeTransform = FPCamera.transform.parent; // Apply shake to parent (CameraShakeHolder)
        Quaternion originalRotation = shakeTransform.localRotation; // Save original rotation
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float xRotation = Random.Range(-1f, 1f) * magnitude;
            float yRotation = Random.Range(-1f, 1f) * magnitude;

            // Apply shake effect without affecting main camera's rotation logic
            shakeTransform.localRotation = originalRotation * Quaternion.Euler(xRotation, yRotation, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset the rotation of the CameraShakeHolder
        shakeTransform.localRotation = originalRotation;
        isShaking = false;
    }

    public void shakeOnShoot()
    {
        if (FPCamera == null)
        {
            Debug.LogError("No camera assigned for shake effect!");
            return;
        }

        if (!isShaking) // Only start a new shake if none is active
        {
            StartCoroutine(CameraShake(shakeDuration, shakeMagnitude));
        }
    }

    public void Reload()
    {
        int ammoInReserve = ammoSlot.GetCurrentAmmo(ammoType);

        if (ammoInReserve > 0 && currentClipAmmo < clipSize)
        {
            int ammoNeeded = clipSize - currentClipAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, ammoInReserve);

            currentClipAmmo += ammoToReload;
            ammoSlot.ReduceCurrentAmmo(ammoType, ammoToReload);

            if (reloadingWeaponSound != null)
            {
                pistolSfx.PlayOneShot(reloadingWeaponSound);
            }

            //Debug.Log($"Reloaded! Ammo: {currentClipAmmo}/{ammoSlot.GetCurrentAmmo(ammoType)}");
        }
        else
        {
            if (weaponClipEmptySfx != null) {
                pistolSfx.PlayOneShot(weaponClipEmptySfx);
            }
            Debug.Log($"No ammo left to reload!");
        }

    }

    IEnumerator StartRecoil()
    {
        // working recoil using animation;
        Animator weaponAnimator = animatorWeaponRef.GetComponent<Animator>();
        string recoilAnimation;
        string idleAnimation;

        if (weaponName == "SMG")
        {
            recoilAnimation = "SMG Recoil";
            idleAnimation = "SMG Idle";
        }
        else if (weaponName == "Sniper_Rifle")
        {
            recoilAnimation = "Sniper_Rifle_Recoil";
            idleAnimation = "Sniper_Rifle_Idle";
        }
        else if (weaponName == "Shotgun")
        {
            recoilAnimation = "Shotgun_Recoil";
            idleAnimation = "Shotgun_Idle";
        }
        else if (weaponName == "SciFiGunLightBlue")
        {
            recoilAnimation = "SciFi_Gun_LightBlue_Recoil";
            idleAnimation = "SciFi_Gun_LightBlue_Idle";
        }
        else if (weaponName == "Crossbow")
        {
            recoilAnimation = "Crossbow_Recoil";
            idleAnimation = "Crossbow_Idle";
        }
        //else if (weaponName == "Minigun")
        //{
        //    Debug.Log("Minigun recoil");
        //    recoilAnimation = "AN_Shooting";
        //    idleAnimation = "Idle";
        //}
        else
        {
            recoilAnimation = "PistolRecoil";
            idleAnimation = "PistolIdle";
        }

        weaponAnimator.Play(recoilAnimation);
        weaponAnimator.ResetTrigger(recoilAnimation);
        yield return new WaitForSeconds(timeBetweenShots);
        weaponAnimator.Play(idleAnimation);
        // Woking recoil using animation end
    }

    void SetIdleAnimation()
    {
        Animator weaponAnimator = animatorWeaponRef.GetComponent<Animator>();

        // Set the appropriate idle animation based on the weapon
        if (weaponName == "SMG")
        {
            weaponAnimator.Play("SMG Idle");
        }
        else if (weaponName == "Sniper_Rifle_Idle")
        {
            weaponAnimator.Play("Sniper_Rifle_Idle");
        }
        else if (weaponName == "Shotgun")
        {
            weaponAnimator.Play("Shotgun_Idle");
        }
        else if (weaponName == "Sniper_Rifle_Idle")
        {
            weaponAnimator.Play("Sniper_Rifle_Idle");
        }
        else if (weaponName == "Crossbow")
        {
            weaponAnimator.Play("Crossbow_Idle");
        }
        else if (weaponName == "Minigun")
        {
            weaponAnimator.Play("Idle");
        }
        else
        {
            weaponAnimator.Play("PistolIdle");
        }
    }

    private void PlayPistolSFX()
    {
        if (pistolSfx != null)
        {
            pistolSfx.PlayOneShot(clip);
        }
        else
        {
            Debug.Log($"No audio clip assigned to pistolSfx for {gameObject.name}");
        }
        

        if (afterShotSfx != null)
        {
            StartCoroutine(playSecondGunSound());
        }
        
    }

    IEnumerator playSecondGunSound()
    {
        yield return new WaitForSeconds(reloadSfxDelay);

        if (pistolSfx != null)
        {
            pistolSfx.PlayOneShot(afterShotSfx);
        }
    }

    IEnumerator playThirdGunSound()
    {
        yield return new WaitForSeconds(shellsFallingDelay);
        if (pistolSfx != null)
        {
            pistolSfx?.PlayOneShot(thirdGunSfx);
        }
    }



    // TODO: remove original particle muzzle flash
    //private void PlayMuzzleFlash()
    //{
    //   muzzleFlash.Play();
    //}

    private void PlayImpactSfx() {
        audioManager.Play("BulletImpact");
    }

    // BLOOD *************************************************************************

    Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }
        return closestBone;
    }

    public Vector3 direction;
    int effectIdx;

    // BLOOD *******************************************************************************
    private void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, GetShootingDirection(), out hit, range))
        {
            try
            {
                CreateHitImpact(hit);
                //TODO: add some visual hit effects
                EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
                if (target == null) return;
                PlayImpactSfx();

                // BLOOD START ********************************************************************
                float angle = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180;
                //var effectIdx = Random.Range(0, BloodFX.Length);
                if (effectIdx == BloodHandler.BloodFX.Length) effectIdx = 0;

                var instance = Instantiate(BloodHandler.BloodFX[effectIdx], hit.point, Quaternion.Euler(0, angle + 90, 0));
                effectIdx++;


                var settings = instance.GetComponent<BFX_BloodSettings>(); // reference to the settings
                                                                           //settings.FreezeDecalDisappearance = InfiniteDecal;
                                                                           //settings.LightIntensityMultiplier = DirLight.intensity;

                var enemyGroundHeight = target.transform.position.y;
                settings.GroundHeight = enemyGroundHeight;

                //settings.GroundHeight = 50f; // TODO: make variable to track floor position around Enemy. Use this as the groundheight for each instance?

                // example: settings.GroundHeight = 


                var nearestBone = GetNearestObject(hit.transform.root, hit.point);
                if (nearestBone != null)
                {
                    var direction = hit.normal;
                    var attachBloodInstance = Instantiate(BloodAttach);
                    var bloodT = attachBloodInstance.transform;
                    bloodT.position = hit.point;
                    bloodT.localRotation = Quaternion.identity;
                    bloodT.localScale = Vector3.one * UnityEngine.Random.Range(0.75f, 1.2f);
                    bloodT.LookAt(hit.point + hit.normal, direction);
                    bloodT.Rotate(90, 0, 0);
                    bloodT.transform.parent = nearestBone;
                    //Destroy(attachBloodInstance, 20);
                }

                // BLOOD END ********************************************************************
                target.TakeDamage(damage); // TODO: player takes damage
            }
            catch (Exception e) {
                // Handle error here
                Debug.Log("Error: " + e.Message);
            }
        }
        else
        {
            return;
        }
    }
    
    // TODO: is this needed?
    //private void CreateHitBloodParticles(RaycastHit hit)
    //{
    //    GameObject impactBlood = Instantiate(hitEffectBlood, hit.point, Quaternion.LookRotation(hit.normal));
    //    //TODO: Add destroy particle - Set impactBlood gameObject to the blood VFX from Asset store
    //}


    private void CreateHitImpact(RaycastHit hit)
    {
        var effect = GetImpactEffect(hit.transform.gameObject);
        if (effect == null)
            return;

        var effectIstance = Instantiate(effect, hit.point, new Quaternion()) as GameObject;
        effectIstance.transform.LookAt(hit.point + hit.normal);
        Destroy(effectIstance, 20);

        Vector3 muzzleTransform = currentWeaponMuzzleRef.transform.position; // Position of where to instantiate the muzzle flash from

        var impactEffectIstance = Instantiate(ImpactEffect, muzzleTransform, transform.rotation) as GameObject;

        audioManager.Play("BulletHitDirt"); // TODO: change sound based on material

        Destroy(impactEffectIstance, 4);

        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }

    //private Vector3 GetShootingDirection()
    //{
    //    Vector3 shakeOffset = new Vector3(
    //        Random.Range(-inaccuracyDistance, inaccuracyDistance) * 0.01f,
    //        Random.Range(-inaccuracyDistance, inaccuracyDistance) * 0.01f,
    //        0
    //    );

    //    return FPCamera.transform.forward + shakeOffset;
    //}

    Vector3 GetShootingDirection()
    {
        Vector3 targetPos = cam.position + cam.forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.y + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.z + Random.Range(-inaccuracyDistance, inaccuracyDistance)
            );

        Vector3 direction = targetPos - cam.position;
        return direction.normalized;

    }
}
