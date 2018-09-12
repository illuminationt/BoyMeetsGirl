using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase<T> where T:MonoBehaviour {



    public virtual void enter(T owner) { }
    public virtual void exit(T owner) { }
    public abstract StateBase<T> update(T owner);
}
