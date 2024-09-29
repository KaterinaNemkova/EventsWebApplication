namespace EventsWebApplication.Core.Enums
{
    public enum AddToEventResult
    {
        Success,        // Успешно добавлен
        AlreadyExists,  // Уже добавлен
        EventNotFound,  // Событие не найдено
        UserNotFound,// Участник не найден
        Failure
    }
}
