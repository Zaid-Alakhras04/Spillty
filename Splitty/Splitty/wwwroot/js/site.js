let tripModal = null;

window.addEventListener('DOMContentLoaded', () => {
    tripModal = new bootstrap.Modal(document.getElementById('createTripModal'));
});

function openCreateTripModal() {
    if (tripModal) {
        tripModal.show();
    }
}

function closeCreateTripModal() {
    if (tripModal) {
        tripModal.hide();
    }
}

function addMemberInputRow() {
    const container = document.getElementById('memberInputsContainer');
    const row = document.createElement('div');
    row.className = 'input-group mb-2 member-input-row';
    row.innerHTML = `
        <input type="text" name="memberNames" class="form-control rounded-3 border-light-subtle bg-light" placeholder="Name" required />
        <button type="button" class="btn btn-outline-danger border-0 px-3" onclick="removeMemberInputRow(this)">Remove</button>
    `;
    container.appendChild(row);
}

function removeMemberInputRow(button) {
    const container = document.getElementById('memberInputsContainer');
    const rows = container.getElementsByClassName('member-input-row');

    if (rows.length > 1) {
        button.closest('.member-input-row').remove();
    } else {
        alert("Every group needs at least one member to keep track of splits.");
    }
}







    

        




