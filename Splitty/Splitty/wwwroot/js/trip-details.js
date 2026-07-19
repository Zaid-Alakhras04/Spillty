var expenseModal = null;

window.addEventListener('DOMContentLoaded', () => {
    expenseModal = new bootstrap.Modal(document.getElementById('addExpenseModal'));
});

function openExpenseModal() {
    if (expenseModal) {
        expenseModal.show();
    }
}

function closeExpenseModal() {
    if (expenseModal) {
        expenseModal.hide();
    }
}

function validateAndSubmit() {
    var checkboxes = document.querySelectorAll('.split-checkbox')
    var errorMessage = document.getElementById('checkboxError');
    var form = document.getElementById('expenseForm');
    var isChecked = false;

    checkboxes.forEach(box => {
        if (box.checked) {
            isChecked = true;
        }
    });

    if (!isChecked) {
        errorMessage.classList.remove('d-none');
        return
    }
    else {
        errorMessage.classList.add('d-none');
    }


    if (form.checkValidity()) {
        form.submit();
    }
    else {
        form.reportValidity();    }
}