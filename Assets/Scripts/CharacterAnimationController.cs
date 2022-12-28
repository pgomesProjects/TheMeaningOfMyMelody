using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator _animator;
    private string characterName = "Player";

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void CharacterDirection(string name)
    {
        _animator.SetBool(name, true);
        _animator.Play(characterName + name);
    }

    public void ResetCharacterDirection(string name)
    {
        _animator.SetBool(name, false);
    }
}
