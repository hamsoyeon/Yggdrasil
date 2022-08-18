using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EMainProtocol = Net.Protocol.EMainProtocol;
using EProtocolType = Net.Protocol.EProtocolType;
public class RoomManager : Singleton_Ver2.Singleton<RoomManager>
{

    private enum EErrType
    {
        NONE,
        ERR_MAXENTER,
        ERR_PW,
        ERR_ROOMINDEX,
    }
    public enum ESubProtocol
    {
        NONE = -1,
        RoomEnter,
        RoomResult,

        MAX

    }
    private RoomInfo m_roominfo;

    #region Initialize
    public void __Initialize()
    {

    }
    #endregion
    #region sendfunc
    #endregion

    #region recv func
    public bool RecvProcess(Net.RecvPacket _recvpacket, Net.Protocol _protocol)
    {
        uint sub_protocol = _protocol.GetProtocol(EProtocolType.Sub);
        bool result = false;
        switch ((ESubProtocol)sub_protocol)
        {
            case ESubProtocol.RoomResult:
                result=EnterResult(_recvpacket);
                break;
        }
        return result;
    }
    private bool EnterResult(Net.RecvPacket _recvpacket)
    {
        int datasize = 0;
        int err_type = -1;
        _recvpacket.Read(out datasize);
        _recvpacket.Read(out err_type);
        switch((EErrType)err_type)
        {
            case EErrType.NONE:
                //입장 성공
                //방 정보 복사하기.
                m_roominfo = new RoomInfo();
                _recvpacket.ReadSerialize(out m_roominfo);

                MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Lobby, false);
                MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Room, true);
                return true;
            case EErrType.ERR_MAXENTER:
                //입장실패 메세지 띄우기
                return false;
            case EErrType.ERR_PW:
                //입장실패 메세지 띄우기
                return false;
            case EErrType.ERR_ROOMINDEX:
                //입장실패 메세지 띄우기
                return false;
            default:
                return false;
        }
    }
    #endregion
    #region func

    #endregion
}
