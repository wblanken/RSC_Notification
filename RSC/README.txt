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


IDEAS:
- Check to see the number of RSC notifications in the folder.	
	- If exceeds X then create a folder and start the process.
- Check the creation time of the files to see if they came before or after approximately 3:00am 
- Figure out a way to check if the previous folder was processed or not and move them over.
	- This will be tricky, could be something like user removes the zip file when it's copied to the USB key but seems risky.
- Possibly create a USB copy portion
	- Could list the available USB devices or prompt the user to insert one and enter the drive letter (or some variation)
	- This could also tie back to the processed requests portion.
- Would like to implement all this in Python/IronPython, want to start using Python for more automation tasks.
- Should keep track of the numbering scheme on the server, can update the ## with the appropriate #'s. 
	- This could be as simple as just do it once and it will always keep track but depends if the user wants to change scheme's ever.
	- Thinking either 00 - 99 or 00 - FF scheme would be sufficient.

Future:
Would like to see the entire process automated.
Could use an email system.
 1. Assign an email address to the server
 2. Forward RSC notification emails to the server
 3. Process them as they are received.
 4. If there's an error with the process, email details to team members involved and halt.