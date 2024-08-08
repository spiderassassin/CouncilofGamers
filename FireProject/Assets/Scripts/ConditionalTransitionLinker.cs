using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionalTransitionLinker : MonoBehaviour
{
    // Fix to allow transitions the work when we disable an intermediate button.
    
    public Selectable target;
    public NavData normal, conditioned;
    public GameObject checkActive;
    
    private Navigation nav;
    private bool wasActive;

    private void OnValidate()
    {
        normal.SetFrom(target);
    }
    private void Awake()
    {
        nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;

        target.navigation = nav;
    }

    private void OnEnable()
    {
        normal.Apply(ref nav);
        target.navigation = nav;
    }

    private void Update()
    {
        bool active = checkActive.activeSelf;
        if(wasActive != active)
        {
            if (active)
            {
                normal.Apply(ref nav);
            }
            else
            {
                conditioned.Apply(ref nav);
            }
            target.navigation = nav;
        }
        wasActive = active;
    }

    [System.Serializable]
    public class NavData
    {
        public Selectable up, down, left, right;

        public void Apply(ref Navigation nav)
        {
            nav.selectOnDown = down;
            nav.selectOnUp = up;
            nav.selectOnRight = right;
            nav.selectOnLeft = left;
        }
        public void SetFrom(Selectable s)
        {
            up = s.navigation.selectOnUp;
            down = s.navigation.selectOnDown;
            right = s.navigation.selectOnRight;
            left = s.navigation.selectOnLeft;
        }
    }
}
