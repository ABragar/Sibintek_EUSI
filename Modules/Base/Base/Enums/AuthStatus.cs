using Base.Attributes;

namespace Base.Enums
{
    [UiEnum]
    public enum AuthStatus
    {
        [UiEnumValue("Успешный вход в систему")]
        Success,
        [UiEnumValue("Пользователь не найден")]
        NotFound,
        [UiEnumValue("Требуется подтверждение")]
        NeedConfirm,
        [UiEnumValue("Пользователь заблокирован")]
        LockedOut,
        [UiEnumValue("Отказ входа в систему")]
        Failure,
        [UiEnumValue("Пользователь вышел из системы")]
        SignOut

        ,// Расширение для windows авторизации
        [UiEnumValue("Отказ входа в систему. Пользователь не состоит ни в одной из разрешенных групп Active Directory.")]
        FailureNotInRole
    }
}