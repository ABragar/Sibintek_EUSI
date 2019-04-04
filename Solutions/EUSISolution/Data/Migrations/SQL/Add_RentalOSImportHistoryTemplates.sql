DECLARE @group INT;

SELECT TOP 1 @group = [ID] 
FROM [CorpProp.Settings].[NotificationGroup] 
WHERE [Name] = N'Импорт'

IF NOT EXISTS (SELECT * FROM [CorpProp.Settings].[UserNotificationTemplate] 
                   WHERE [Code] = N'RentalOSImportHistory_Success')
INSERT INTO [CorpProp.Settings].[UserNotificationTemplate] 
([Oid]
, [ObjectType]
, [Title]
, [Code]
, [Subject]
, [Message]
, [Description]
, [IsHTML]
, [HtmlBody]
, [BySystem]
, [ByEmail]
, [ReportID]
, [NotificationGroupID]
, [Hidden]
, [SortOrder]
, [Recipient]
)
VALUES (
NEWID()
, N'EUSI.Entities.Accounting.RentalOS, EUSI'
, N'Уведомление об успешном испорте ФСД (Аренда)'
, N'RentalOSImportHistory_Success'
, N'Уведомление о результате импорта'
, N''
, N''
, 1
, N'<!DOCTYPE html>
<html>
<head>
<title></title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<style type="text/css">
    /* FONTS */
    @media screen {
        @font-face {
          font-family: ''Lato'';
          font-style: normal;
          font-weight: 400;
          src: local(''Lato Regular''), local(''Lato-Regular''), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format(''woff'');
        }
        
        @font-face {
          font-family: ''Lato'';
          font-style: normal;
          font-weight: 700;
          src: local(''Lato Bold''), local(''Lato-Bold''), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format(''woff'');
        }
        
        @font-face {
          font-family: ''Lato'';
          font-style: italic;
          font-weight: 400;
          src: local(''Lato Italic''), local(''Lato-Italic''), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format(''woff'');
        }
        
        @font-face {
          font-family: ''Lato'';
          font-style: italic;
          font-weight: 700;
          src: local(''Lato Bold Italic''), local(''Lato-BoldItalic''), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format(''woff'');
        }
    }
    
    /* CLIENT-SPECIFIC STYLES */
    body, table, td, a { -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }
    table, td { mso-table-lspace: 0pt; mso-table-rspace: 0pt; }
    img { -ms-interpolation-mode: bicubic; }

    /* RESET STYLES */
    img { border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; }
    table { border-collapse: collapse !important; }
    body { height: 100% !important; margin: 0 !important; padding: 0 !important; width: 100% !important; }

    /* iOS BLUE LINKS */
    a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
    }
    
    /* MOBILE STYLES */
    @media screen and (max-width:600px){
        h1 {
            font-size: 32px !important;
            line-height: 32px !important;
        }
    }

    /* ANDROID CENTER FIX */
    div[style*="margin: 16px 0;"] { margin: 0 !important; }
</style>
</head>
<body style="background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;">

<!-- HIDDEN PREHEADER TEXT -->
<div style="display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: ''Lato'', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;">
    Уведомление об успешном импорте
</div>

<table border="0" cellpadding="0" cellspacing="0" width="100%">
    
    <tr>
        <td colspan="2" bgcolor="#66BB7F" align="center">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;" >
                <tr>
                    <td align="center" valign="top" style="padding: 40px 10px 40px 10px;"></td>
                </tr>
            </table>
        </td>
    </tr>
    <!-- HEADER -->
    <tr>
        <td colspan="2" bgcolor="#ffffff" align="center" valign="top" style="border-bottom: solid 3px #FFD700; background-color: #f4f4f4; padding: 0px 10px 10px 10px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 1px; line-height: 48px;">
            <h1 style="font-size: 24px; font-weight: 400; margin: 0;">{Fields.Subject}</h1>
        </td>
    </tr>
    
    <tr>
		<td bgcolor="#ffffff" align="right" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
			<p style="margin: 0; padding-top: 20px;"><b>Дата/время импорта:</b></p>
		</td>
		<td bgcolor="#ffffff" align="left" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;" >
			<p style="margin: 0; padding-top: 20px;">{Fields.ImportDateTime}</p>
        </td>		
    </tr>	
	<tr>
		<td bgcolor="#ffffff" align="right" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;" >
			<p style="margin: 0;"><b>Наименование файла:</b></p>
        </td>
		<td bgcolor="#ffffff" align="left" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;" >
			<p style="margin: 0; word-wrap: normal">{Fields.FileName}</p>
        </td>
	</tr>
	<tr>
		<td bgcolor="#ffffff" align="right" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
			<p style="margin: 0;"><b>Результат:</b></p>
        </td>
		<td bgcolor="#ffffff" align="left" style="width: 50%; vertical-align: top; padding: 0px 30px 20px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;" >
			<p style="margin: 0;">{Fields.ResultText}</p>
		</td>
	</tr>
    <!-- FOOTER -->
    <tr>
        <td colspan="2" bgcolor="#ffffff" align="center" valign="top" style="border-top: solid 3px #FFD700; background-color: #f4f4f4; padding: 0px 10px 10px 10px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400;  line-height: 48px;">
            <h1 style="font-size: 18px; font-weight: 400; margin: 0; padding-top: 10px;">2018 ООО СИБИНТЕК-СОФТ</h1>
        </td>
    </tr>
</table>

</body>
</html>
', 
0, 
1, 
NULL, 
@group, 
0, 
0, 
NULL)

IF NOT EXISTS (SELECT * FROM [CorpProp.Settings].[UserNotificationTemplate] 
                   WHERE [Code] = N'RentalOSImportHistory_Fail')
INSERT INTO [CorpProp.Settings].[UserNotificationTemplate] 
([Oid]
, [ObjectType]
, [Title]
, [Code]
, [Subject]
, [Message]
, [Description]
, [IsHTML]
, [HtmlBody]
, [BySystem]
, [ByEmail]
, [ReportID]
, [NotificationGroupID]
, [Hidden]
, [SortOrder]
, [Recipient]
)
VALUES (
NEWID()
, N'EUSI.Entities.Accounting.RentalOS, EUSI'
, N'Уведомление об отмене импорта ФСД (Аренда)'
, N'RentalOSImportHistory_Fail'
, N'Уведомление о результате импорта'
, N''
, N''
, 1
, N'<!DOCTYPE html>
<html>
<head>
<title></title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<style type="text/css">
    /* FONTS */
    @media screen {
        @font-face {
          font-family: ''Lato'';
          font-style: normal;
          font-weight: 400;
          src: local(''Lato Regular''), local(''Lato-Regular''), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format(''woff'');
        }
        
        @font-face {
          font-family: ''Lato'';
          font-style: normal;
          font-weight: 700;
          src: local(''Lato Bold''), local(''Lato-Bold''), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format(''woff'');
        }
        
        @font-face {
          font-family: ''Lato'';
          font-style: italic;
          font-weight: 400;
          src: local(''Lato Italic''), local(''Lato-Italic''), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format(''woff'');
        }
        
        @font-face {
          font-family: ''Lato'';
          font-style: italic;
          font-weight: 700;
          src: local(''Lato Bold Italic''), local(''Lato-BoldItalic''), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format(''woff'');
        }
    }
    
    /* CLIENT-SPECIFIC STYLES */
    body, table, td, a { -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }
    table, td { mso-table-lspace: 0pt; mso-table-rspace: 0pt; }
    img { -ms-interpolation-mode: bicubic; }

    /* RESET STYLES */
    img { border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; }
    table { border-collapse: collapse !important; }
    body { height: 100% !important; margin: 0 !important; padding: 0 !important; width: 100% !important; }

    /* iOS BLUE LINKS */
    a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
    }
    
    /* MOBILE STYLES */
    @media screen and (max-width:600px){
        h1 {
            font-size: 32px !important;
            line-height: 32px !important;
        }
    }

    /* ANDROID CENTER FIX */
    div[style*="margin: 16px 0;"] { margin: 0 !important; }
</style>
</head>
<body style="background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;">


    <div style="display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: ''Lato'', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;">
        Уведомление об отклонении импорта
    </div>

<table border="0" cellpadding="0" cellspacing="0" width="100%">
    
    <tr>
        <td colspan="2" bgcolor="#FF8C00" align="center">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;" >
                <tr>
                    <td align="center" valign="top" style="padding: 40px 10px 40px 10px;"></td>
                </tr>
            </table>
        </td>
    </tr>
    <!-- HEADER -->
    <tr>
        <td colspan="2" bgcolor="#ffffff" align="center" valign="top" style="border-bottom: solid 3px #FFD700; background-color: #f4f4f4; padding: 0px 10px 10px 10px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 1px; line-height: 48px;">
            <h1 style="font-size: 24px; font-weight: 400; margin: 0;">{Fields.Subject}</h1>
        </td>
    </tr>
    
    <tr>
		<td bgcolor="#ffffff" align="right" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
			<p style="margin: 0; padding-top: 20px;"><b>Дата/время импорта:</b></p>
		</td>
		<td bgcolor="#ffffff" align="left" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;" >
            <p style="margin: 0; padding-top: 20px;">{Fields.ImportDateTime}</p>
        </td>		
    </tr>	
	<tr>
		<td bgcolor="#ffffff" align="right" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;" >
			<p style="margin: 0;"><b>Наименование файла:</b></p>
        </td>
		<td bgcolor="#ffffff" align="left" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;" >
            <p style="margin: 0; word-wrap: normal;">{Fields.FileName}</p>
        </td>
	</tr>
	<tr>
		<td bgcolor="#ffffff" align="right" style="width: 50%; vertical-align: top; padding: 0px 30px 5px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
			<p style="margin: 0;"><b>Результат:</b></p>
        </td>
		<td bgcolor="#ffffff" align="left" style="width: 50%; vertical-align: top; padding: 0px 30px 20px 30px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;" >
            <p style="margin: 0;">{Fields.ResultText}</p>
		</td>
	</tr>
	<tr>
		<td colspan="2" bgcolor="#ffffff" align="center" style="padding: 0px 40px 30px 40px;">
            <table border="1" bordercolor="#C0C0C0" cellpadding="0" cellspacing="0" width="100%" style="" >
				<thead>
					<th bgcolor="#FFFFFF" align="center" style="padding: 5px 5px 5px 20px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;">Номер колонки</th>
					<th bgcolor="#FFFFFF" align="center" style="padding: 5px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;">Номер строки</th>
					<th bgcolor="#FFFFFF" align="center" style="padding: 5px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;">Наименование</th>
					<th bgcolor="#FFFFFF" align="center" style="padding: 5px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;">Текст ошибки</th>
					<th bgcolor="#FFFFFF" align="center" style="padding: 5px 20px 5px 5px; color: #000000; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 25px;">Тип ошибки</th>
				</thead>
                    <tbody>
                        {List.Fields}<tr>
                            <td bgcolor="#FFFFFF" align="center" style="padding: 0px 10px 5px 30px; color: #666666; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: 400; line-height: 25px;">
                                <p style="margin: 0;">{List.ImportErrorLogs.ColumnNumber}</p>
                            </td>
                            <td bgcolor="#FFFFFF" align="center" style="padding: 0px 10px 5px 10px; color: #666666; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: 400; line-height: 25px;">
                                <p style="margin: 0;">{List.ImportErrorLogs.RowNumber}</p>
                            </td>
                            <td bgcolor="#FFFFFF" align="center" style="padding: 0px 10px 5px 10px; color: #666666; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: 400; line-height: 25px;">
                                <p style="margin: 0;">{List.ImportErrorLogs.PropetyName}</p>
                            </td>
                            <td bgcolor="#FFFFFF" align="center" style="padding: 0px 10px 5px 10px; color: #666666; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: 400; line-height: 25px;">
                                <p style="margin: 0;">{List.ImportErrorLogs.ErrorText}</p>
                            </td>
                            <td bgcolor="#FFFFFF" align="center" style="padding: 0px 30px 5px 10px; color: #666666; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: 400; line-height: 25px;">
                                <p style="margin: 0;">{List.ImportErrorLogs.ErrorType}</p>
                            </td>
                        </tr>{/List.Fields}
                    </tbody>
            </table>
		</td>
	</tr>
    <!-- FOOTER -->
    <tr>
        <td colspan="2" bgcolor="#ffffff" align="center" valign="top" style="border-top: solid 3px #FFD700; background-color: #f4f4f4; padding: 0px 10px 10px 10px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: ''Lato'', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400;  line-height: 48px;">
            <h1 style="font-size: 18px; font-weight: 400; margin: 0; padding-top: 10px;">2018 ООО СИБИНТЕК-СОФТ</h1>
        </td>
    </tr>
</table>

</body>
</html>
'
,0
,1
,NULL
, @group
, 0
, 0
,NULL)