#include <stdio.h>

#include <iostream>
#include <string>

#ifdef _LINUX
#include "luxthread.h"
#elif _UTTEST  //lint -e553
#include "winthread_stub.h"
#else
#include "windows/winthread.h"
#endif

#include "logexception.h"

#include "logqueue.h"

#include "recorder.h"
#include "ofstreammanager.h"

#include "loginfo.h"

using std::cout;

const ushort MAX_LOG_BUFF_LEN = 10*1024;

CLogRecorder::CLogRecorder() : 
    m_pThread(NULL), 
    m_bExist(false),
    m_pLogQueue(NULL), 
    m_pOstreamManager(NULL), 
    m_ulLogBuffLen(MAX_LOG_BUFF_LEN),
    m_pLogBuff(new char[m_ulLogBuffLen])
{

}

CLogRecorder::~CLogRecorder()
{
    delete []m_pLogBuff;
    m_pLogBuff = NULL;
}

void CLogRecorder::Initialize(CLogQueue* pLogQueue, COstreamManager* pOstreamManager)
{
    m_pLogQueue = pLogQueue;
    m_pOstreamManager = pOstreamManager;
}

void CLogRecorder::StartThread()
{
    m_pThread = new CCurrPlatformThread;
    if( !m_pThread->Initialize(ThreadFunc, this, NULL) || !m_pThread->StartThread())
    {
        throw CLogException("start log record thread failed");
    }
}

void CLogRecorder::ThreadFunc(void* pParam)
{
     CLogRecorder* pLogRecorder = static_cast<CLogRecorder *>(pParam);
     if (pLogRecorder == NULL)
     {
         return;
     }

     while(!pLogRecorder->m_bExist)
     {
        pLogRecorder->Record();
     }
}

void CLogRecorder::Record()
{
    if (m_pLogQueue == NULL || m_pOstreamManager == NULL)
    {
        m_bExist = true;
        return;
    }

    (void)m_pLogQueue->GetLog(m_pLogBuff, m_ulLogBuffLen);

    size_t LogLen = strlen(m_pLogBuff);
    if (LogLen != 0)
    {
        ostream& OutStream = m_pOstreamManager->GetCurrOutStream(LogLen);

        OutStream << m_pLogBuff;
		OutStream.flush();
    }

    m_pLogBuff[0] = '\0';
    m_ulLogBuffLen = MAX_LOG_BUFF_LEN;
}

void CLogRecorder::StopThread()
{
    if (m_pLogQueue == NULL || m_pThread == NULL)
    {
        return;
    }

    m_bExist = true;
    (void)SetEvent(m_pLogQueue->m_hPutLogEvent);

    m_pThread->WaitEnd();
}