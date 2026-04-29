using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class RandomPetrify : MonoBehaviour
{
    
    [SerializeField] private float _minDuration = 5f, _maxDuration = 12f;
    [SerializeField] private int _unpetrifyKeySpamCount = 10;
    
    [SerializeField] private InputAction _unpetrifyKeySpamAction;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PetrifyOverlayController _overlay;
    
    private bool _continueRandomPetrify = true;
    private int _currKeySpamCount = 0;

    private void OnEnable()
    {
        GameEnding.OnEnd += () => _continueRandomPetrify = false;
    }

    private void OnDisable()
    {
        GameEnding.OnEnd -= () => _continueRandomPetrify = false;
    }
    
    private void Start()
    {
        StartCoroutine(RandomPetrifyRoutine());
    }

    private void IncrementKeySpamCount(InputAction.CallbackContext ctx)
    {
        _currKeySpamCount++;
        print($"Courage {_currKeySpamCount}!!!");
        _overlay.Heartbeat(_currKeySpamCount);
    }
    
    private void EnableKeySpam(bool enabled)
    {
        _currKeySpamCount = 0;
        if (enabled)
        {
            _unpetrifyKeySpamAction.Enable();
            _unpetrifyKeySpamAction.started += IncrementKeySpamCount;
            return;
        }
        _unpetrifyKeySpamAction.Disable();
        _unpetrifyKeySpamAction.started -= IncrementKeySpamCount;
    }


    private IEnumerator RandomPetrifyRoutine()
    {
        while (_continueRandomPetrify)
        {
            float randDur = Random.Range(_minDuration, _maxDuration);
            print($"Wait for {randDur} seconds");
            yield return new WaitForSeconds(randDur);
            print($"Wait while Petrified...");
            yield return Petrify();
        }
    }

    private IEnumerator Petrify()
    {
        _playerMovement.InterruptMovement(true);
        EnableKeySpam(true);
        _overlay.Activate(true);
        yield return new WaitUntil(() => _currKeySpamCount >= _unpetrifyKeySpamCount);
        _playerMovement.InterruptMovement(false);
        EnableKeySpam(false);
        _overlay.Activate(false);
    }
}
