DECLARE @settingsItemId INT;

	--Создаем настройку типа "Аудит"
	IF NOT EXISTS (SELECT ID FROM Settings.SettingItem WHERE Title = N'Аудит' AND [Hidden] = 0)
	BEGIN
		INSERT INTO Settings.SettingItem (Title, Hidden, SortOrder)
		VALUES (N'Аудит', 0, 1)
	END

	SET @settingsItemId = (SELECT ID FROM Settings.SettingItem WHERE Title = N'Аудит' AND [Hidden] = 0)

	--Включаем аудит
	IF NOT EXISTS (SELECT ID FROM [Audit].AuditSetting WHERE ID = @settingsItemId)
	BEGIN
		INSERT INTO [Audit].AuditSetting (ID, MaxRecordCount, RegisterLogIn)
		VALUES (@settingsItemId, -1, 1)
	END

	--Включаем аудит для сущности EstateRegistration
	IF NOT EXISTS (SELECT ID FROM [Audit].AuditSettingEntity WHERE AuditSetting_ID = @settingsItemId 
						AND [Hidden] = 0
						AND FullName = N'EUSI.Entities.Estate.EstateRegistration, EUSI')
	BEGIN
		INSERT INTO [Audit].AuditSettingEntity (FullName, [Hidden], SortOrder, AuditSetting_ID)
		VALUES (N'EUSI.Entities.Estate.EstateRegistration, EUSI', 0, -1, @settingsItemId)
	END