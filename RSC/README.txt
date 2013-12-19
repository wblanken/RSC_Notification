This application is for automatically processing RSC notifications received by email and packaging them for processing on the server.

Currently this application will create a folder for today's date then process any notifications saved into that folder manually by the user. 
It will generate the appropriate list.txt and zip archive of the directory.



TODO:
Through using the following Outlook macro it is possible to automatically save emails as they are sent: 
	
	Sub SaveAsText(RSC As MailItem)
		Dim name As String
		Dim path As String

		name = RSC.Subject + ".txt"
		path = '<save location> ' Make sure to update the path to the local system (e.g. "C:\\RSC\\")
    
		RSC.SaveAs path + name, olTXT    
	End Sub
	
Using this I want to run some logic that runs around the same time as the mirror. 
Can check to see how many messages there are waiting as well as what would have arrived before the mirroring process began.
Then this application could be run off the task scheduler at about the same time of day as the mirror, 
when the user arrives in the morning the archive would already be created and waiting.