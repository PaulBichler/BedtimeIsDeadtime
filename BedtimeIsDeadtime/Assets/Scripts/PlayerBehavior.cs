using System;
using System.Collections;
using AI;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class PlayerBehavior : MonoBehaviour
{
    public int speed;
    private Rigidbody2D _obj;
    [SerializeField]
    private int power = 0, powerMax = 100;
    [HideInInspector] public float _paralyzed = .0f;
    
    //ultimate related variables
    private FieldOfView _flashlightRef;
    private Light2D _ultimateLight;
    public float ultimateIntensity = 0.8f;
    public float maxUltimateRadius;
    public float ultimateLerpSpeed = 2f;
    public float ultimateLerpDuration = 2f;
    [HideInInspector] public bool isUltimateInUse = false;


    private SpriteRenderer _spriteRenderer;    
    
    private Sprite _forwardSprite;
    private Sprite _backwardSprite;
    private Sprite _sideSprite;
    
    private Camera _camera1;
    private Transform _flashlightTransform;

    // Start is called before the first frame update

    private void Awake()
    {
        Game.Player = this;
    }
    
    private void Start()
    {
        _forwardSprite = Resources.Load<Sprite>("Sprites/PlayerForward");
        _backwardSprite = Resources.Load<Sprite>("Sprites/PlayerBackward");
        _sideSprite = Resources.Load<Sprite>("Sprites/PlayerSide");
        _flashlightTransform = transform.Find("PlayerFlashlight").transform;

        Game.Hud.power.value = power;
        _flashlightRef = GetComponentInChildren<FieldOfView>();
        _ultimateLight = transform.Find("PlayerUltimateLight").GetComponent<Light2D>();
        _camera1 = Camera.main;
        _obj = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_paralyzed <= 0)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            
            Vector3 tempVect = new Vector3(h, v, 0);
            tempVect = speed * Time.deltaTime * tempVect.normalized;
            _obj.MovePosition(_obj.transform.position + tempVect);

            if (Input.GetButtonDown("Ultimate") && power > 0 && !isUltimateInUse)
            {
                isUltimateInUse = true;
                StartCoroutine("Ultimate");
            }
        }
        else
        {
            if((_paralyzed -= Time.deltaTime) < 0)
            {
                _paralyzed = 0;
            }
        }
        

        if (_flashlightTransform.up.y < -0.5f)
        {
            _spriteRenderer.sprite = _forwardSprite;
        }
        else if(_flashlightTransform.up.y > 0.5f)
        {
            _spriteRenderer.sprite = _backwardSprite;
        }
        else
        {
            if (_flashlightTransform.rotation.z > 0f)
            {
                _spriteRenderer.sprite = _sideSprite;
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.sprite = _sideSprite;
                _spriteRenderer.flipX = false;
            }
        }
    }

    public void Paralyze(float paralyzeTime)
    {
        _paralyzed += paralyzeTime;
    }

    public void AddPower(Transform enemy)
    {
        AI.EnemyBehaviour enemyScript = enemy.GetComponent<EnemyBehaviour>();

        if (!enemyScript) return;
        power += enemyScript.rewardedPower;

        if (power > powerMax) power = powerMax;
        Game.Hud.power.value = power;
    }

    IEnumerator Ultimate()
    {
        yield return LerpDetectRadius(0f,(maxUltimateRadius / 100) * power, ultimateLerpDuration);
        yield return LerpDetectRadius(_ultimateLight.pointLightOuterRadius, 0f, ultimateLerpDuration);
    }

    IEnumerator LerpDetectRadius(float startRadius, float endRadius, float time)
    {
        float i = (endRadius > startRadius) ? 0f : 1f;
        float rate = (1f / time) * ultimateLerpSpeed;
        float direction = (endRadius > startRadius) ? 1f : -1f;
        float lerpValue = (endRadius > startRadius) ? endRadius : startRadius;

        while (i <= 1f && i >= 0f)
        {
            i += Time.deltaTime * rate * direction;
            _ultimateLight.intensity = i * ultimateIntensity;
            _ultimateLight.pointLightOuterRadius = lerpValue * i;
            _ultimateLight.pointLightInnerRadius = lerpValue * i / 4;
            yield return null;
        }

        if (_ultimateLight.intensity <= 0f)
        {
            isUltimateInUse = false;
            yield return null;
        }
        else
        {
            yield return FindTargetsWithDelay(0.2f);
        }
    }

    IEnumerator FindTargetsWithDelay(float delay) {
        while (power > 0) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            power -= 2;
            Game.Hud.power.value = power;
        }

        yield return null;
    }

    void FindVisibleTargets() {
        Collider2D[] targetsInHitRadius = Physics2D.OverlapCircleAll(transform.position, _ultimateLight.pointLightOuterRadius, _flashlightRef.targetMask);

        for (int i = 0; i < targetsInHitRadius.Length; i++) 
        {
            if (!targetsInHitRadius[i].CompareTag("Enemy"))
            {
                continue;
            }

            if (targetsInHitRadius[i].transform)
            {
                Transform target = targetsInHitRadius[i].transform;
                EnemyBehaviour targetScript = target.GetComponent<EnemyBehaviour>();
                if (target.localScale.x > targetScript.deathScale)
                {
                    target.localScale -= new Vector3(targetScript.scaleDecreaseFactor, targetScript.scaleDecreaseFactor, 0f);
                }
                else
                {
                    Game.Bed.AddSanity(target);
                    target.GetComponent<EnemyBehaviour>().Stop();
                    Destroy(target.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SanityPickup"))
        {
            Game.Bed.AddSanity(15);
            Destroy(other.gameObject);
        } 
    }
}
