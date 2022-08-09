using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorManager : Singleton_Ver2.Singleton<SectorManager>
{
    [SerializeField]
    Transform m_parent;
    [SerializeField]
    LineRenderer m_line_prefeb;
    private float m_start_x;
    private float m_end_x;
    private float m_start_z;
    private float m_end_z;
    private const float m_pos_y = 3;
    private const float m_distance = 15.5f;

    private List<LineRenderer> m_lines;

    void Start()
    {
        m_start_x = -45;
        m_end_x = 195;
        m_start_z = 38;
        m_end_z = -142;

        float startpos = m_start_x;
        //vertical
        for (int i = 0; i < 8 * 2; i++)
        {
            LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
            item.SetPosition(0, new Vector3(startpos, m_pos_y, m_start_z));
            item.SetPosition(1, new Vector3(startpos, m_pos_y, m_end_z));
            startpos += m_distance;
        }
        startpos = m_start_z;
        //horizontal
        for (int i = 0; i < 6 * 2; i++)
        {
            LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
            item.SetPosition(0, new Vector3(m_start_x, m_pos_y, startpos));
            item.SetPosition(1, new Vector3(m_end_x, m_pos_y, startpos));
            startpos -= m_distance;
        }

    }
    //private const float m_h_distance = 240 / 3;
    //private const float m_v_distance = 180 / 3;
    //private List<LineRenderer> m_lines;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    m_start_x = -45;
    //    m_end_x = 195;
    //    m_start_z = 38;
    //    m_end_z = -142;

    //    float startpos = m_start_x;
    //    //vertical
    //    for (int i = 0; i < 4; i++)
    //    {
    //        LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
    //        item.SetPosition(0, new Vector3(startpos, m_pos_y, m_start_z));
    //        item.SetPosition(1, new Vector3(startpos, m_pos_y, m_end_z));
    //        startpos += m_h_distance;
    //    }
    //    startpos = m_start_z;
    //    //horizontal
    //    for (int i = 0; i < 4; i++)
    //    {
    //        LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
    //        item.SetPosition(0, new Vector3(m_start_x, m_pos_y, startpos));
    //        item.SetPosition(1, new Vector3(m_end_x, m_pos_y, startpos));
    //        startpos -= m_v_distance;
    //    }

    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
