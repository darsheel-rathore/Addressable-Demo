using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuPlayerAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Animator>().SetTrigger(Animator.StringToHash("Dancing"));
    }

    private void OnDisable()
    {
        GetComponent<Animator>().ResetTrigger(Animator.StringToHash("Dancing"));
    }
}
