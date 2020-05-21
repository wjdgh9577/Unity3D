using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    public GameObject upNote;
    public GameObject downNote;
    public GameObject leftNote;
    public GameObject rightNote;
    public GameObject spaceNote;
    float time, oneBit;
    int notes, dx, dz;

    // Start is called before the first frame update
    void Start()
    {
        this.time = 0;
        this.oneBit = 60 / Global.BPM;
        this.notes = 1;
        this.dx = this.dz = 0;
    }

    private void MakeNote(int notes, float timing, char type)
    {
        if (this.time >= timing && this.notes == notes)
        {
            GameObject note;
            if (type == 'u')
            {
                note = Instantiate(upNote) as GameObject;
                note.transform.Translate(dx, Global.speed, dz);
                this.dz += 1;
            }
            else if (type == 'd')
            {
                note = Instantiate(downNote) as GameObject;
                note.transform.Translate(dx, Global.speed, dz);
                this.dz -= 1;
            }
            else if (type == 'l')
            {
                note = Instantiate(leftNote) as GameObject;
                note.transform.Translate(dx, Global.speed, dz);
                this.dx -= 1;
            }
            else if (type == 'r')
            {
                note = Instantiate(rightNote) as GameObject;
                note.transform.Translate(dx, Global.speed, dz);
                this.dx += 1;
            }
            else if (type == 's')
            {
                note = Instantiate(spaceNote) as GameObject;
                note.transform.Translate(dx, Global.speed, dz);
            }
            this.time = 0;
            this.notes++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        this.time += Time.deltaTime;
        MakeNote(1, 1.85f, 'l');//0
        MakeNote(2, 0.138125f, 'l');
        MakeNote(3, 0.138125f*2, 'l');
        MakeNote(4, 0.138125f*2, 'l');
        MakeNote(5, 0.138125f*2, 'u');
        MakeNote(6, 0.138125f*2, 'u');
        MakeNote(7, 0.138125f*2, 'u');
        MakeNote(8, 0.138125f*2, 'u');
        MakeNote(9, 0.138125f*2, 'r');
        MakeNote(10, 0.138125f, 'r');//2.21
        MakeNote(11, 0.13f, 'r');
        MakeNote(12, 0.13f*2, 'r');
        MakeNote(13, 0.13f*2, 'r');
        MakeNote(14, 0.13f*2, 'r');
        MakeNote(15, 0.13f*2, 'r');
        MakeNote(16, 0.13f*2, 'r');
        MakeNote(17, 0.13f, 'd');
        MakeNote(18, 0.13f, 'd');
        MakeNote(19, 0.13f, 'd');
        MakeNote(20, 0.13f*2, 'd');//2.08
        MakeNote(21, 0.133125f, 'd');
        MakeNote(22, 0.133125f*2, 'd');
        MakeNote(23, 0.133125f*2, 'd');
        MakeNote(24, 0.133125f*2, 'd');
        MakeNote(25, 0.133125f*2, 'l');
        MakeNote(26, 0.133125f*2, 'l');
        MakeNote(27, 0.133125f*2, 'l');
        MakeNote(28, 0.133125f*2, 'l');
        MakeNote(29, 0.133125f, 'l');//2.13
        MakeNote(30, 0.136875f, 'l');
        MakeNote(31, 0.136875f*2, 'l');
        MakeNote(32, 0.136875f, 'l');
        MakeNote(33, 0.136875f, 'u');
        MakeNote(34, 0.136875f*2, 'u');
        MakeNote(35, 0.136875f, 'u');
        MakeNote(36, 0.136875f*2, 'u');
        MakeNote(37, 0.136875f*2, 'u');
        MakeNote(38, 0.136875f/2, 'u');
        MakeNote(39, 0.136875f/2, 'u');
        MakeNote(40, 0.136875f/2, 'u');
        MakeNote(41, 0.136875f/2, 'r');
        MakeNote(42, 0.136875f/2, 'r');
        MakeNote(43, 0.136875f/2, 'r');
        MakeNote(44, 0.136875f/2, 'r');
        MakeNote(45, 0.136875f/2, 'r');//2.19
        /*MakeNote(6, 2.19f, 's');
        MakeNote(7, 2.13f, 's');
        MakeNote(8, 2.09f, 's');
        MakeNote(9, 2.18f, 's');
        MakeNote(10, 2.18f, 's');
        MakeNote(11, 2.13f, 's');
        MakeNote(12, 2.16f, 's');
        MakeNote(13, 2.14f, 's');
        MakeNote(14, 2.23f, 's');
        MakeNote(15, 2.10f, 's');
        MakeNote(16, 2.16f, 's');
        MakeNote(17, 2.18f, 's');
        MakeNote(18, 2.06f, 's');
        MakeNote(19, 2.21f, 's');
        MakeNote(20, 2.16f, 's');
        MakeNote(21, 2.20f, 's');
        MakeNote(22, 2.09f, 's');
        MakeNote(23, 2.10f, 's');
        MakeNote(24, 2.14f, 's');
        MakeNote(25, 2.22f, 's');
        MakeNote(26, 2.14f, 's');
        MakeNote(27, 2.18f, 's');
        MakeNote(28, 2.11f, 's');
        MakeNote(29, 2.13f, 's');
        MakeNote(30, 2.16f, 's');
        MakeNote(31, 2.16f, 's');
        MakeNote(32, 2.16f, 's');
        MakeNote(33, 2.15f, 's');
        MakeNote(34, 2.20f, 's');
        MakeNote(35, 2.12f, 's');
        MakeNote(36, 2.17f, 's');
        MakeNote(37, 2.15f, 's');
        MakeNote(38, 2.14f, 's');
        MakeNote(39, 2.16f, 's');
        MakeNote(40, 2.15f, 's');
        MakeNote(41, 1.16f, 's');
        MakeNote(42, 1.01f, 's');
        MakeNote(43, 1.08f, 's');
        MakeNote(44, 1.04f, 's');
        MakeNote(45, 1.09f, 's');
        MakeNote(46, 1.00f, 's');
        MakeNote(47, 1.13f, 's');
        MakeNote(48, 1.09f, 's');
        MakeNote(49, 1.06f, 's');
        MakeNote(50, 1.14f, 's');
        MakeNote(51, 2.13f, 's');
        MakeNote(52, 2.13f, 's');
        MakeNote(53, 2.13f, 's');
        MakeNote(54, 2.15f, 's');
        MakeNote(55, 2.15f, 's');
        MakeNote(56, 2.10f, 's');
        MakeNote(57, 2.17f, 's');
        MakeNote(58, 2.15f, 's');
        MakeNote(59, 2.17f, 's');
        MakeNote(60, 2.15f, 's');
        MakeNote(61, 2.11f, 's');
        MakeNote(62, 2.20f, 's');
        MakeNote(63, 2.16f, 's');
        MakeNote(64, 2.12f, 's');
        MakeNote(65, 2.20f, 's');
        MakeNote(66, 2.12f, 's');
        MakeNote(67, 2.17f, 's');
        MakeNote(68, 2.10f, 's');
        MakeNote(69, 2.13f, 's');
        /*MakeNote(70, this.oneBit, 'u');
        MakeNote(71, this.oneBit, 'u');
        MakeNote(72, this.oneBit, 'u');*/
    }
}
