#ifndef LOG_QUEUE_FSS_2013_02_13
#define LOG_QUEUE_FSS_2013_02_13

#include "buildintype.h" //lint -e537

#include "singleton.h"
#include "circlebuff.h"

#ifdef _LINUX
#include "luxevent.h"
#endif

#ifdef _UTTEST
#include "ut_stub.h"
#endif

class CLogInfo;

class CLogQueue
{
    friend class CLogRecorder;

public:
    CLogQueue();
    ~CLogQueue();

    void Initialize();

    ushort PutLog(const char* pLogBuff, ushort usLen);
    ushort GetLog(char* pLogBuff, ulong& ulLen);

    bool IsEmpty();

private:
    CCircleBuff m_CircleBuff;

    HANDLE m_hPutLogEvent;
    time_t m_LastReadTime;
};

typedef CSingleton<CLogQueue> CLogQueueS;

const ulong  CHECK_QUEUE_TIME = 1;

#ifdef _UTTEST 
const ulong  MAX_QUEUE_BUFF_LEN = 2*1024+1;
#else
const ulong  MAX_QUEUE_BUFF_LEN = 1024*1024;
#endif


#endif
