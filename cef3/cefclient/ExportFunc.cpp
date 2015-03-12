#include "CommonModule.h"
#include "ExportFuncs.h"  
//--------------------------------
using namespace std;  
//--------------------------------
//static
del02 managedListner;
del03 managedListner3;

int MyCefGetVersion()
{	
	 return 1001;
}
//-----------------------------------------------------------
int RegisterManagedCallBack(void* funcPtr,int callbackKind)
{	
	switch(callbackKind)
	{
		case 0:
			{
				managedListner= (del02)funcPtr;		    
				return 0;
			}break;
		case 1:
			{
			 
			}break;
		case 3:
			{
				managedListner3= (del03)funcPtr;		    
				return 0;
			}break;
	} 
	return 1;
}
//-----------------------------------------------------------
void ManagedCallBack(int id,const wchar_t* info)
{
	 if(managedListner)
	 {
		 managedListner(id,info);
	 } 
}

const wchar_t* ManagedCallBack3(int id,const wchar_t* info)
{
	 if(managedListner3)
	 {
		return managedListner3(id,info);
	 } 
	 return NULL;
}
bool HasManagedCallBack()
{
	if(managedListner)
	{
		return true;
	}
	else
	{
		return false;
	}
}
//ClientHandler* MyCefCreateClientHandler()
//{	
//	return new ClientHandler();
//}
ClientHandler* MyCefCreateClientHandler()
{	
	return new ClientHandler();
}
void MyCefRunMessageLoop()
{
	 CefRunMessageLoop();
}
int MyCefShutDown()
{
	CefShutdown();
	return 0;
} 
bool MyCefUseMultiMessageLoop()
{
	return IsMultiMessageLoopApp();
}
 
int MyCefInit(HINSTANCE hInstance)//,CefSettings* settings)
{	 
	return MyAppInit01(hInstance);  
}

 void MyCefDoMessageLoopWork()
 {		
	CefDoMessageLoopWork();
	//MSG msg;
	//int j = 10;
	// 
	//while(PeekMessage(&msg, NULL, 0, 0,PM_REMOVE)){
	//	   //if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg)) {
	//	TranslateMessage(&msg);
	//	DispatchMessage(&msg); 
	//	CefDoMessageLoopWork();
	//}
 }  
 void MyCefQuitMessageLoop()
{
	CefQuitMessageLoop();

}

//-----
//easy ...
void NavigateTo(ClientHandler* g_ClientHandler, const wchar_t* url)
{    
	auto browser = g_ClientHandler->GetBrowser(); 
	browser->GetMainFrame()->LoadURL(CefString(url)); 
}
void ExecJavascript(ClientHandler* g_ClientHandler, 
	const wchar_t* jssource,
	const wchar_t* scripturl)
{
	auto browser = g_ClientHandler->GetBrowser(); 
	browser->GetMainFrame()->ExecuteJavaScript(CefString(jssource),
		CefString(scripturl),0);
} 
void PostData(ClientHandler* g_ClientHandler, 
	const wchar_t* url,
	void* rawDataToPost,
	size_t rawDataLength)
{

	 // Create a new request
  CefRefPtr<CefRequest> request(CefRequest::Create());
  
  //Set the request URL
  //eg. request->SetURL("http://tests/request");
  request->SetURL(url);

  // Add post data to the request.  The correct method and content-
  // type headers will be set by CEF.
  
  CefRefPtr<CefPostDataElement> postDataElement(CefPostDataElement::Create());   
  
  //eg.  
  /*std::string data = "arg1=val1&arg2=val2";
  postDataElement->SetToBytes(data.length(), data.c_str());*/

  postDataElement->SetToBytes(rawDataLength,rawDataToPost);
  CefRefPtr<CefPostData> postData(CefPostData::Create());
  postData->AddElement(postDataElement);
  request->SetPostData(postData);

  // Add a custom header
  CefRequest::HeaderMap headerMap;
  headerMap.insert(
      std::make_pair("X-My-Header", "My Header Value"));
  request->SetHeaderMap(headerMap);

  // Load the request
  auto browser = g_ClientHandler->GetBrowser(); 
  browser->GetMainFrame()->LoadRequest(request); 
}
 


class MyCefStringVisitor : public CefStringVisitor {
   public:
    explicit MyCefStringVisitor(CefRefPtr<CefBrowser> browser, del04 strCallBack) : 
			browser_(browser),
			strCallBack_(strCallBack)
			{}
    virtual void Visit(const CefString& string) OVERRIDE {		  
		strCallBack_(0,string.c_str()); 
    }
   private:
    CefRefPtr<CefBrowser> browser_;
	del04  strCallBack_;
    IMPLEMENT_REFCOUNTING(Visitor);
}; 

void DomGetTextWalk(ClientHandler* g_ClientHandler,del04 strCallBack)
{ 
  auto browser1 = g_ClientHandler->GetBrowser();   
  //TODO: check mem leak here
  auto visitor= new MyCefStringVisitor(browser1,strCallBack); 
  browser1->GetMainFrame()->GetText(visitor);

}
void DomGetSourceWalk(ClientHandler* g_ClientHandler,del04 strCallBack)
{ 
  auto browser1 = g_ClientHandler->GetBrowser();   
  //TODO: check mem leak here
  auto visitor= new MyCefStringVisitor(browser1,strCallBack); 
  browser1->GetMainFrame()->GetSource(visitor);
}
void AgentRegisterManagedCallback(ClientHandler* g_ClientHandler,managed_callback callback)
{
	g_ClientHandler->SetManagedCallBack(callback);
}

	  
void CefCallbackArgsSetOutputString(CefCallbackArgs* args,const void* outputBuffer,int len)
{
	args->SetOutputString(outputBuffer,len);
} 
void CefCallbackArgsGetInputString(CefCallbackArgs* args,wchar_t* resultBuffer,int* len)
{	 
    auto wstr= args->input;
	*len = wcslen(wstr);
	wcscpy(resultBuffer, wstr);
}