#pragma once

// 맨 끝에 있는 좌표값.
// 원점부터 맵의 크기를 판단하기 위해,
// obj의 위치값에 좌표값을 더해줌.
#define MAX_X 15 
#define MAX_Y 15

#define MAX_SIZE 3

class CSession;
class CSector;

class CSectorMgr
{
public:
	static CSectorMgr* GetInst();
	static void Create();
	static void Destroy();

private:
	static CSectorMgr* m_instance;

	CSectorMgr();
	~CSectorMgr();

	// 맵정보
public:
	// 플레이어,몬스터,obj ... 샌드 패킷, <- 상호작용 관련
	// 플레이어 샌드 패킷
	// 몬스터 샌드 패킷
	// 아이템 샌드 패킷
	void PlayerSendPacket(CSession* _ptr, int _protocol, bool _moveflag);
	void MonsterSendPacket(CSession* _ptr, int _protocol, bool _moveflag); // id를 전송하면 클라에선 id로 구분하여 몬스터를 이동.
	void ItemSendPacket(CSession* _ptr, int _protocol, bool _on_off_flag); // 

	void Init(); // Sector 초기화
	void AddSectorList(CSession* _ptr);
	void RemoveSectorList();

private:
	list<CSector*> m_sectorList;

};

// 섹터의 전체를 관리.

// 캐릭터, 몹 관련 패킷처리 매니저를 구현.
// 하나의 스테이트에서 진행.
// 잡몹, 캐릭터, 보스처리 패킷인지 구분.
// 프로토콜 매니저에서 서브쯤에 분리해서 보고, 프로토콜이 어느것이 더 많을지 보고, -> 플레이어가 패킷처리가 많을 것 같다..

// 초기 obj스폰위치
// 몹, 쫄, 보스, 플레이어가 움직임.
// 보스만 타일의 제약을 받고(패턴은 서버에서 랜덤으로 구현할 수 도 있음.)
// 셋 중 하나를 호스트로 삼아서, AI는 그 안에서만 돌려서, 쫄이 움직였다 뭐가 되었다 하면 서버에 호스트가 보낸다.
// 렌더링 전에 움직일 값을 서버에 보낸다.
// 위치값을 가지고 서버에 보내면, 서버가 이것을 받아서 이 세명에게 같이 뿌린다.
// 뿌려진 값으로 렌더링을 한다. // 그럼 동시성이 보인다.
// 몹이랑 싸워서 이겼다는 검증만 한다. 죽였다는 패킷만 서버에 보내면 사정거리 검증후 인증패킷을 보낸다.
// 로컬이 아니 곳에서 서버를 돌리면 딜레이가 생기므로 클라쪽에서 정보를 저장할 코드를 구현해야함.
