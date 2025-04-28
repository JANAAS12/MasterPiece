// Admin Panel Custom JavaScript

$(document).ready(function () {
    // Initialize tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Sidebar active item highlighting
    var url = window.location;
    $('ul.pcoded-item li a').filter(function () {
        return this.href == url;
    }).parent().addClass('active').closest('.pcoded-hasmenu').addClass('pcoded-trigger');

    // Notification counter
    updateNotificationCount();

    // Real-time updates (example)
    setInterval(function () {
        updateNotificationCount();
    }, 30000); // Update every 30 seconds
});

function updateNotificationCount() {
    $.ajax({
        url: '/Admin/GetNotificationCount',
        type: 'GET',
        success: function (data) {
            $('.header-notification .badge').text(data.count);
        }
    });
}

// Full screen toggle
function toggleFullScreen() {
    if (!document.fullscreenElement) {
        document.documentElement.requestFullscreen();
    } else {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        }
    }
}

// Logout confirmation
$('.logout-btn').click(function (e) {
    e.preventDefault();
    if (confirm('Are you sure you want to logout?')) {
        window.location.href = $(this).attr('href');
    }
});