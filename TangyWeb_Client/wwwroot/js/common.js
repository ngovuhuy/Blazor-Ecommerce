window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Operation successful", { timeOut: 5000 });
    }
    if (type === "error") {
        toastr.error(message, "Operation fail", { timeOut: 5000 });
    }
    if (type === "warning") {
        toastr.warning(message, "Operation warning", { timeOut: 5000 });
    }
}

window.ShowSwal = (type, message) => {
    if (type === "success") {
        Swal.fire(
            'Success Notification!',
            message,
           'success'
        );
    }
    if (type === "error") {
        Swal.fire(
            'Error Notification!',
            message,
            'error'
        );
    }
   
}

function ShowDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('show');
}

function HideDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('hide');
}

