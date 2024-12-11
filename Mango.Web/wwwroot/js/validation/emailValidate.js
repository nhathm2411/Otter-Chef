document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('forgotPasswordForm');
    const emailInput = document.getElementById('email');
    const emailError = document.getElementById('emailError');

    form.addEventListener('submit', function (event) {
        const emailValue = emailInput.value.trim();
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;  // Mẫu định dạng email cơ bản

        // Kiểm tra nếu email không hợp lệ
        if (!emailPattern.test(emailValue)) {
            event.preventDefault(); // Ngăn không cho form được submit
            emailError.textContent = 'The email format is invalid. Please enter it again!';
        } else {
            emailError.textContent = ''; // Xóa thông báo lỗi nếu email hợp lệ
        }
    });
});