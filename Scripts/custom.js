

$(document).on('change', '.category-select', (e) => {
    const id = $(e.target).val();
    console.log(id)
    $.ajax({
        type: 'GET',
        url: `Categories/GetSubCategories/${id}`,
        dataType: 'json',
        success: function (data) {
            console.log(data)
        },
        error: function (data) {

        }
    })

})
