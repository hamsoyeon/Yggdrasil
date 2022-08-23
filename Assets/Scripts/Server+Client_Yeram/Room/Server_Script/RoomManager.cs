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
        NONE_ANOTHER_ENTER,
        ERR_MAXENTER,
        ERR_PW,
        ERR_ROOMINDEX,
        ERR_CHARACTER, //이미 선택 완료한 캐릭터
    }
    public enum ESubProtocol
    {
        NONE = 0,
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
    private PlayerInfo m_myinfo;
    public PlayerInfo PlayerInfo
    {
        get => m_myinfo;
    }
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
        protocol.SetProtocol((int)ESubProtocol.NONE, EProtocolType.Sub);
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
    public void ReadyProcess(bool _ready)
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((int)EMainProtocol.ROOM, EProtocolType.Main);
        protocol.SetProtocol((int)ESubProtocol.Multi, EProtocolType.Sub);
        protocol.SetProtocol((int)EDetailProtocol.ReadySelect, EProtocolType.Detail);
        protocol.SetProtocol((int)EDetailProtocol.NomalReady, EProtocolType.Detail);

        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        int size = sendpacket.Write(m_roominfo.GetID);
        size += sendpacket.Write(_ready);
        sendpacket.WriteProtocol(protocol.GetProtocol());
        sendpacket.WriteTotalSize(size);
        Net.NetWorkManager.Instance.Send(sendpacket);
    }
    public void ChattingProcess(string _text)
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((int)EMainProtocol.ROOM, EProtocolType.Main);
        protocol.SetProtocol((int)ESubProtocol.Multi, EProtocolType.Sub);
        protocol.SetProtocol((int)EDetailProtocol.ChatSend, EProtocolType.Detail);
        protocol.SetProtocol((int)EDetailProtocol.AllMsg, EProtocolType.Detail);

        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        int size = sendpacket.Write(m_roominfo.GetID);
        size+=sendpacket.Write(_text);
        sendpacket.WriteProtocol(protocol.GetProtocol());
        sendpacket.WriteTotalSize(size);
        Net.NetWorkManager.Instance.Send(sendpacket);
    }
    public void GameStartProcess(int _mapmode)
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((int)EMainProtocol.ROOM, EProtocolType.Main);
        protocol.SetProtocol((int)ESubProtocol.Multi, EProtocolType.Sub);
        protocol.SetProtocol((int)EDetailProtocol.ReadySelect, EProtocolType.Detail);
        protocol.SetProtocol((int)EDetailProtocol.HostReady, EProtocolType.Detail);
        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        int size = sendpacket.Write(m_roominfo.GetID);
        size += sendpacket.Write(_mapmode);
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
            //호스트 시작 버튼 활성화 ( 모든 유저들 준비 완료상태 )
            case EDetailProtocol.ReadySelect | EDetailProtocol.HostReady:
                HostReadyReq(_recvpacket);
                break;
            case EDetailProtocol.ReadyResult | EDetailProtocol.HostReady:
                HostReadyResult(_recvpacket);
                break;
            case EDetailProtocol.ReadyResult | EDetailProtocol.NomalReady:
                NomalReadyResult(_recvpacket);
                break;
            case EDetailProtocol.ChatRecv | EDetailProtocol.AllMsg:
                ChatRecv(_recvpacket, _protocol);
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
            {   //입장 성공
                    //방 정보 복사하기.
                    //int curid = -1;
                    int myid = -1;
                    PlayerInfo[] another_info = new PlayerInfo[2];
                    PlayerInfo myinfo = null;
                    m_roominfo = new RoomInfo();
                    _recvpacket.ReadSerialize(out m_roominfo);
                    //_recvpacket.Read(out curid);
                    _recvpacket.Read(out myid);
                    int count = 0;
                    foreach (PlayerInfo player in m_roominfo.GetPlayersInfo)
                    {
                        if (player.GetID != myid)
                        {
                            another_info[count++] = player;
                        }
                        else
                        {
                            myinfo = player;
                        }
                    }
                    RoomGUIManager.Instance.SettingSlotInfo(myinfo, another_info[0], another_info[1]);
                    MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Lobby, false);
                    MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Room, true);

                    m_myinfo = myinfo;
                    for (int i=0;i<m_roominfo.GetPlayersInfo.Count;i++)
                    {
                        PlayerInfo player = m_roominfo.GetPlayersInfo[i];
                        RoomGUIManager.Instance.RenderCharImage(player.GetID);
                        RoomGUIManager.Instance.RenderReady(player.GetID, player.GetReady, true);
                    }

                    bool map_flag = false;
                    if(m_roominfo.GetOwner == myid)
                    {
                        map_flag = true;
                    }
                    RoomGUIManager.Instance.EnableMapBtn(map_flag);
                    return true;
            }
            case EErrType.NONE_ANOTHER_ENTER:
            {
                    int myid = -1;
                    PlayerInfo[] another_info = new PlayerInfo[2];
                    PlayerInfo myinfo = null;
                    PlayerInfo player; 
                    _recvpacket.ReadSerialize(out player);
                    m_roominfo.GetPlayersInfo.Add(player);
                    _recvpacket.Read(out myid);
                     int count = 0;
                   
                    foreach (PlayerInfo item in m_roominfo.GetPlayersInfo)
                    {
                        if (item.GetID != myid)
                        {
                            another_info[count++] = item;
                        }
                        else
                        {
                            myinfo = item;
                        }
                    }
                    RoomGUIManager.Instance.SettingSlotInfo(myinfo, another_info[0], another_info[1]);
                    return false;
            }
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
                for(int i=0;i<m_roominfo.GetPlayersInfo.Count;i++)
                {
                    PlayerInfo player = m_roominfo.GetPlayersInfo[i];
                    if (player.GetID==id)
                    {
                        player.GetCharacterInfo.CharacterType = type;
                    }

                }
                //이부분 슬롯을 순회하면서 id와 일치한 슬롯의 인덱스 뽑아와서 id와 바꾸기
                RoomGUIManager.Instance.RenderCharImage(id, type);
                break;
        }
       
        //해당 아이디의 player 정보에 가서 캐릭터 타입 변경해주기.=> 이거는 레디했을 때
        //캐릭터 선택에 대한 확정은 레디했을 때로 하자.

    }
    private void HostReadyReq(Net.RecvPacket _recvPacket)
    {
        int datasize = 0;
        bool allready = false;
        //캐릭터 변경된 player 아이디 받아옴
        _recvPacket.Read(out datasize);
        _recvPacket.Read(out allready);
        RoomGUIManager.Instance.EnableStartBtn(allready);
    }
    private void NomalReadyResult(Net.RecvPacket _recvPacket)
    {
        int datasize = 0;
        int id = -1;
        bool ready = false;
        bool another = false;
        //레디상태 변경 요청한 player 아이디 받아옴
        _recvPacket.Read(out datasize);
        _recvPacket.Read(out id);
        _recvPacket.Read(out ready);
        _recvPacket.Read(out another);
      
        RoomGUIManager.Instance.RenderReady(id, ready,another);
    }
    private void HostReadyResult(Net.RecvPacket _recvPacket)
    {
        int datasize = 0;
      
        bool result = false;
      
        //레디상태 변경 요청한 player 아이디 받아옴
        _recvPacket.Read(out datasize);
        _recvPacket.Read(out result );
      
        if(result)
        {
            //상태 게임으로 진입
        }
        else
        {
            //모두 레디 안한 경우
        }
    }
    public void ChatRecv(Net.RecvPacket _recvpacket, Net.Protocol _protocol)
    {
        string text;
        int datasize = 0;
        bool result = false;
        _recvpacket.Read(out datasize);
        _recvpacket.Read(out result);
        if (result)
        {
            _recvpacket.Read(out text);
            RoomGUIManager.Instance.UpdateChat(text);
        }
        else // 채팅 보내기 실패한 경우 ex) 공백 전송
        {

        }
    }
    #endregion
    #region func

    #endregion
}
