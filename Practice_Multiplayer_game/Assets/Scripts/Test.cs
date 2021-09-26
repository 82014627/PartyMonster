using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int speed;
    Rigidbody rigidbody;
    Vector3 forwarddirection,  rightdirction;
    bool isjump = false;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 glbalDirectionforward = transform.TransformDirection(Vector3.forward);
        Vector3 globaldirectionright = transform.TransformDirection(Vector3.up);
        if (x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            forwarddirection = x * glbalDirectionforward * Time.deltaTime;
        }
        else if (x < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            forwarddirection = -x * glbalDirectionforward * Time.deltaTime;
        }

        if (isjump)
        {
            if (y > 0)
            {
                Debug.LogError("no");
                rigidbody.Sleep();
                rigidbody.AddForce(transform.up * 350000*Time.deltaTime);
                //rightdirction = 1 * globaldirectionright * Time.deltaTime;
                isjump = false;
            }
        }
       

        
      

        Vector3 maindis = forwarddirection;//+ rightdirction

        rigidbody.MovePosition(rigidbody.position + maindis * speed);

        //float s = 10 * Input.GetAxis("Mouse X");
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Quaternion.AngleAxis(x, Vector3.up).eulerAngles);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isjump = true;
        }
    }
}
