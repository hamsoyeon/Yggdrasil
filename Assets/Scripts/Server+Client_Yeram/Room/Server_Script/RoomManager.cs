using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EMainProtocol = Net.Protocol.EMainProtocol;
using EProtocolType = Net.Protocol.EProtocolType;
using ECharacterType = CharacterInfo.ECharacterType;

public class RoomManager : Singleton_Ver2.Singleton<RoomManager>
{

    private enum EErrType
    {
        NONE,
        ERR_MAXENTER,
        ERR_PW,
        ERR_ROOMINDEX,
        ERR_CHARACTER, //이미 선택 완료한 캐릭터
    }
    public enum ESubProtocol
    {
        NONE = -1,
        Init,
        Multi,
        Single,
        BackPage,
        MAX
    }
    public enum EDetailProtocol
    {
        NONE = -1,
        //========비트 중복 가능========= 3bit
        RoomEnter,
        RoomResult,
        CharacterSelect,
        CharacterResult,
        MapSelect,
        MapResult,
        //========비트 중복 불가능========= 4bit
        ReadySelect = 8,
        ReadyResult = 16,
        ChatSend = 32,
        ChatRecv = 64,
        //========비트 중복 불가능========= 4bit
        HostReady = 128,
        NomalReady = 256,
        NoticeMsg = 512,
        AllMsg = 1024,
        MAX
    }
    private RoomInfo m_roominfo;

    #region Initialize
    public void __Initialize()
    {

    }
    #endregion
    #region sendfunc
    public void EnterRoomProcess(uint _roomid, string _pw)
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((int)EMainProtocol.ROOM, EProtocolType.Main);

        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        int size = sendpacket.Write(_roomid);
        size += sendpacket.Write(_pw);
        sendpacket.WriteProtocol(protocol.GetProtocol());
        sendpacket.WriteTotalSize(size);
        Net.NetWorkManager.Instance.Send(sendpacket);
    }
    public void CharacterSelectProcess(ECharacterType _character_type)
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((int)EMainProtocol.ROOM, EProtocolType.Main);
        protocol.SetProtocol((int)ESubProtocol.Multi, EProtocolType.Sub);
        protocol.SetProtocol((int)EDetailProtocol.CharacterSelect, EProtocolType.Detail);

        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        //방번호
        int size = sendpacket.Write(m_roominfo.GetID);
        //캐릭터 타입
        size += sendpacket.Write((int)_character_type);
        sendpacket.WriteProtocol(protocol.GetProtocol());
        sendpacket.WriteTotalSize(size);
        Net.NetWorkManager.Instance.Send(sendpacket);
    }
    #endregion

    #region recv func
    public bool RecvProcess(Net.RecvPacket _recvpacket, Net.Protocol _protocol)
    {
        uint sub_protocol = _protocol.GetProtocol(EProtocolType.Sub);
        bool result = false;
        switch ((ESubProtocol)sub_protocol)
        {
            case ESubProtocol.Init:
                break;
            case ESubProtocol.Multi:
                result = MultiRecvProcess(_recvpacket, _protocol);
                break;
            case ESubProtocol.Single:
                SingleRecvProcess(_recvpacket, _protocol);
                break;
        }
        return result;
    }
    private bool MultiRecvProcess(Net.RecvPacket _recvpacket, Net.Protocol _protocol)
    {
        bool result = false;
        uint detail_protocol = _protocol.GetProtocol(EProtocolType.Detail);
        switch ((EDetailProtocol)detail_protocol)
        {
            case EDetailProtocol.RoomResult:
                result = EnterResult(_recvpacket);
                break;
            case EDetailProtocol.CharacterResult:
                CharacterResult(_recvpacket);
                break;
            case EDetailProtocol.MapResult:
                break;
            case EDetailProtocol.ReadyResult | EDetailProtocol.HostReady:
                break;
            case EDetailProtocol.ReadyResult | EDetailProtocol.NomalReady:
                break;
            case EDetailProtocol.ChatRecv | EDetailProtocol.AllMsg:
                break;
            case EDetailProtocol.ChatRecv | EDetailProtocol.NoticeMsg:
                break;


        }
        return result;
    }
    private void SingleRecvProcess(Net.RecvPacket _recvpacket, Net.Protocol _protocol)
    {

    }
    private bool EnterResult(Net.RecvPacket _recvpacket)
    {
        int datasize = 0;
        int err_type = -1;
        _recvpacket.Read(out datasize);
        _recvpacket.Read(out err_type);
        switch ((EErrType)err_type)
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
    private void CharacterResult(Net.RecvPacket _recvPacket)
    {
        int datasize = 0;
        int id = 0;
        int type_number = 0;
        int result = -1;
        //캐릭터 변경된 player 아이디 받아옴
        _recvPacket.Read(out datasize);
        _recvPacket.Read(out result);
        _recvPacket.Read(out id);
        _recvPacket.Read(out type_number);
        switch((EErrType)result)
        {
            case EErrType.ERR_CHARACTER:
                break;
            case EErrType.NONE:
                ECharacterType type = (ECharacterType)type_number;
                //이부분 슬롯을 순회하면서 id와 일치한 슬롯의 인덱스 뽑아와서 id와 바꾸기
                RoomGUIManager.Instance.RenderCharImage(id, type);
                break;
        }
       
        //해당 아이디의 player 정보에 가서 캐릭터 타입 변경해주기.=> 이거는 레디했을 때
        //캐릭터 선택에 대한 확정은 레디했을 때로 하자.

    }
    #endregion
    #region func

    #endregion
}
