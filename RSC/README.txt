This application is for automatically processing RSC notifications received by email and packaging them for processing on the server.

Through using the following Outlook macro it is possible to automatically save emails as they are sent: 

		Sub SaveAsText(RSC As MailItem)
			Dim name As String
			Dim path As String
    
			' Remove bad characters in the RSC notification subject lines
			name = GetValidName(RSC.Subject)

			' Make sure to update the path to the local system (e.g. "C:\RSC\")
			RSC.SaveAs "E:\RSC\" & name & ".txt", olTXT
    
		End Sub

		Function GetValidName(name As String)
			' File Name cannot have these \ / : * ? " < > |
			Dim sTemp As String
    
			sTemp = name
    
			sTemp = Replace(sTemp, "\", "")
			sTemp = Replace(sTemp, "/", "")
			sTemp = Replace(sTemp, ":", "")
			sTemp = Replace(sTemp, "*", "")
			sTemp = Replace(sTemp, "?", "")
			sTemp = Replace(sTemp, """", "")
			sTemp = Replace(sTemp, "<", "")
			sTemp = Replace(sTemp, ">", "")
			sTemp = Replace(sTemp, "|", "")
    
			GetValidName = sTemp
		End Function

When this application is run it will process all notifications in the same folder as the .exe and create a folder with the current date as the name. 
It will also check other folders to find a .zip archive. If one is found, this signals to the application that the notifications for that day were 
not processed so will add those to the next batch to the process and append the newer ones.

Scheduling this application to run every day around the same time as the mirroring process results in a fairly accurate RSC package to be processed the next business day.


IDEAS:
- [IMPLEMENTED] Check to see the number of RSC notifications in the folder.	 
	- If exceeds X then create a folder and start the process.
- [IMPLEMENTED] Check the creation time of the files to see if they came before or after approximately 3:00am 
- [IMPLEMENTED] Figure out a way to check if the previous folder was processed or not and move them over.
	- This will be tricky, could be something like user removes the zip file when it's copied to the USB key but seems risky.
- Possibly create a USB copy portion
	- Could list the available USB devices or prompt the user to insert one and enter the drive letter (or some variation)
	- This could also tie back to the processed requests portion.
- [IMPLEMENTED] Should keep track of the numbering scheme on the server, can update the ## with the appropriate #'s. 
	- This could be as simple as just do it once and it will always keep track but depends if the user wants to change scheme's ever.
	- Thinking either 00 - 99 or 00 - FF scheme would be sufficient.

Future:
Would like to see the entire process automated.
Could use an email system.
 1. Assign an email address to the server
 2. Forward RSC notification emails to the server
 3. Process them as they are received.
 4. If there's an error with the process, email details to team members involved and halt.