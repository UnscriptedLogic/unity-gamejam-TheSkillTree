using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInspectable
{
    string Name { get; }
    Sprite Icon { get; }

    string GetInspectDesc();
}
