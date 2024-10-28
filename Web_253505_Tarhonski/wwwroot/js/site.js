$(document).ready(function () {
    $(document).on('click', '.page-link', function (e) {
        e.preventDefault();

        // Получаем URL и номер страницы из data-атрибутов
        var url = $(this).data('ajax-url');
        var page = $(this).data('page'); // Убедитесь, что у элемента есть data-page с номером страницы
        var category = '@ViewContext.HttpContext.Request.RouteValues["category"]?.ToString() ?? ""';

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                category: category,
                pageNo: page
            },
            headers: {
                'X-Requested-With': 'XMLHttpRequest' // Явно добавляем заголовок для AJAX-запроса
            },
            success: function (result) {
                // Обновляем блок с товарами и пагинацией
                $('#airsoft-list').html(result);
            },
            error: function () {
                alert('Error loading data.');
            }
        });
    });
});
