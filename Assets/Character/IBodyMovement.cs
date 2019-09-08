using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public interface IBodyMovement
{
    void Move(Vector2 direction);
    void Jump();
    void Crouch();
}

