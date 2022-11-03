using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG;
using SDG.Unturned;
namespace Hag.Misc
{
    class Vehicle :MonoBehaviour 
    {
        void Start()
        {
            StartCoroutine(Flight());
        }

        IEnumerator Flight()
        {
            for (; ; )
            {
                if (Player.player && Provider.isConnected  )
                {
                    InteractableVehicle car = Player.player?.movement?.getVehicle();
                    if (car != null && car && Provider.isConnected && !Provider.isLoading)
                    {
                        Rigidbody rigidbody = car.GetComponent<Rigidbody>();
                        if (Globals.Config.VehicleMisc.VehicleFlight)
                        {
                            rigidbody.constraints = RigidbodyConstraints.None;
                            rigidbody.freezeRotation = false;
                            rigidbody.useGravity = false;
                            rigidbody.isKinematic = true;

                            Transform transform = car.transform;

                            if (Input.GetKey(KeyCode.W))
                                rigidbody.MovePosition(transform.position + transform.forward * (car.asset.speedMax * Time.fixedDeltaTime));

                            if (Input.GetKey(KeyCode.S))
                                rigidbody.MovePosition(transform.position - transform.forward * (car.asset.speedMax * Time.fixedDeltaTime));

                            if (Input.GetKey(KeyCode.A))
                                transform.Rotate(0f, -2f, 0f);

                            if (Input.GetKey(KeyCode.D))
                                transform.Rotate(0f, 2f, 0f);

                            if (Input.GetKey(KeyCode.UpArrow))
                                transform.Rotate(-1.5f, 0f, 0f);

                            if (Input.GetKey(KeyCode.DownArrow))
                                transform.Rotate(1.5f, 0f, 0f);

                            if (Input.GetKey(KeyCode.LeftArrow))
                                transform.Rotate(0f, 0f, 1.5f);

                            if (Input.GetKey(KeyCode.RightArrow))
                                transform.Rotate(0f, 0f, -1.5f);

                            if (Input.GetKey(KeyCode.Q))
                                transform.position = transform.position + new Vector3(0f, .2f, 0f);

                            if (Input.GetKey(KeyCode.E))
                                transform.position = transform.position - new Vector3(0f, .2f, 0f);
                        }
                        else
                        {
                            rigidbody.useGravity = true;
                            rigidbody.isKinematic = false;
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
