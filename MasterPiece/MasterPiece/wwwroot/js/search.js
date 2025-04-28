
$(document).ready(function () {
    $("#searchBtn").click(function () {
        var query = $("#clinicSearch").val().trim(); // الحصول على النص من حقل البحث

        if (query === "") {
            alert("Please enter a search term.");
            return;
        }

        $.ajax({
            url: "/Clinics/SearchClinics",
            type: "GET",
            data: { query: query },
            success: function (clinics) {
                var clinicList = $("#clinicList");
                clinicList.empty(); // مسح القائمة السابقة

                if (clinics.length === 0) {
                    clinicList.append("<p>No clinics found.</p>");
                }

                // بناء الـ HTML للعيادات المسترجعة
                $.each(clinics, function (index, clinic) {
                    var clinicHtml = `
                        <div class="col-lg-4 col-md-6 mb-30 properties-items">
                            <div class="item">
                                <a href="#"><img src="/Img/${clinic.image}" alt="${clinic.name}" style="width: 100%;"></a>
                                <span class="category">${clinic.category}</span>
                                <h6>
                                    <div>${"⭐".repeat(clinic.rating)}</div>
                                </h6>
                                <h4><a href="#">${clinic.name}</a></h4>
                                <div style="display: flex;">
                                    <i class="fa-solid fa-location-dot" style="margin-top: 5px;"></i>
                                    <p style="margin-left: 5px;">${clinic.location}</p>
                                </div>
                                <div style="display: flex;">
                                    <i class="fa-solid fa-clock" style="margin-top: 5px;"></i>
                                    <p style="margin-left: 5px;">${clinic.workingHours}</p>
                                </div>
                                <hr>
                                <div class="main-button">
                                    <a href="#">Schedule a visit</a>
                                </div>
                            </div>
                        </div>`;
                    clinicList.append(clinicHtml);
                });
            },
            error: function () {
                alert("Error fetching search results. Please try again.");
            }
        });
    });
});
