$(document).ready(function () {
    $("#locationFilter .dropdown-item").click(function (e) {
        e.preventDefault();

        var location = $(this).data("location");

        $.ajax({
            url: "/Clinics/GetClinicsByLocation",
            type: "GET",
            data: { location: location },
            success: function (clinics) {
                var clinicList = $("#clinicList");
                clinicList.empty(); // مسح العيادات القديمة

                if (clinics.length === 0) {
                    clinicList.append("<p>No clinics found for this location.</p>");
                }

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
                alert("Error fetching data. Please try again.");
            }
        });
    });
});