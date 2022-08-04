#pragma once
#include "CState.h"
class CInGameState :
    public CState
{
public:
	CInGameState(CSession* _session) :CState(_session) {}
	virtual void Recv() final;
	virtual void Send() final;

};

