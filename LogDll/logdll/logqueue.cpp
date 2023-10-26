#include <time.h>
#include "logexception.h"

#include "logqueue.h"
#include "loginfo.h"

const ulong  MAX_CACHE_LEN  = MAX_LOGINF_LEN*200;
const ushort MAX_CACHE_TIME = 5;

CLogQueue::CLogQueue() : m_CircleBuff(MAX_QUEUE_BUFF_LEN), m_hPutLogEvent(NULL)
{
    (void)time( &m_LastReadTime );
}

CLogQueue::~CLogQueue()
{
    (void)CloseHandle(m_hPutLogEvent);
}

void CLogQueue::Initialize()
{
    m_hPutLogEvent = CreateEvent(NULL, false, false, NULL);
    if (m_hPutLogEvent == NULL)
    {
        throw CLogException("create put event faild");
    }
}

ushort CLogQueue::PutLog(const char* pLogBuff, ushort usLen)
{
    ushort usRet = m_CircleBuff.Write((uchar *)pLogBuff, usLen);
    if (usRet  == CIRC_SUCC)
    {
        if (m_CircleBuff.DataLength() >= MAX_CACHE_LEN)
        {
            (void)SetEvent(m_hPutLogEvent);
        }
    }

    return usRet;
}

ushort CLogQueue::GetLog(char* pLogBuff, ulong& ulLen)
{
    if (m_CircleBuff.IsEmpty())
    {
        while (WaitForSingleObject(m_hPutLogEvent, CHECK_QUEUE_TIME*1000) == WAIT_TIMEOUT)
        {
            time_t CurrTime;
            (void)time( &CurrTime );

            if (CurrTime - m_LastReadTime >= MAX_CACHE_TIME )
            {
                break;
            }
        }
    }
   
    (void)time(&m_LastReadTime);

    ulong  ulReadLen = ulLen-1;
    (void)m_CircleBuff.Read((uchar *)pLogBuff, ulReadLen);

    if (pLogBuff[ulReadLen] != '\0')
    {
        pLogBuff[ulReadLen] = '\0';
    }
    

    ulLen = ulReadLen;

    return 0;
}

bool CLogQueue::IsEmpty()
{
    return m_CircleBuff.IsEmpty();
}