$(function () {
    'use strict';

    function getLast12Months() {
        var labels = [];
        var currentDate = new Date();

        for (var i = 11; i >= 0; i--) {
            var d = new Date(currentDate.getFullYear(), currentDate.getMonth() - i, 1);
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            labels.push( month + '/' + year); 
        }

        return labels;
    }

    var data = {
        labels: getLast12Months(),
        datasets: [{
            label: 'Price',
            data: orderChartLine,
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)',
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)'
            ],
            borderColor: [
                'rgba(255,99,132,1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)',
                'rgba(255,99,132,1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1,
            fill: false
        }]
    };

    var options = {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        },
        legend: {
            display: false
        },
        elements: {
            point: {
                radius: 0
            }
        }
    };

    if ($("#lineChart").length) {
        var lineChartCanvas = $("#lineChart").get(0).getContext("2d");
        var lineChart = new Chart(lineChartCanvas, {
            type: 'line',
            data: data,
            options: options
        });
    }
});

$(function () {
    'use strict';


    function getLast7Days() {
        var labels = [];
        var currentDate = new Date();

        for (var i = 6; i >= 0; i--) {
            var pastDate = new Date();
            pastDate.setDate(currentDate.getDate() - i);

            var day = pastDate.getDate();
            var month = pastDate.getMonth() + 1;
            labels.push('Day: ' + day + '/' + month); 
        }

        return labels;
    }



    var data = {
        labels: getLast7Days(),
        datasets: [{
            label: 'Price',
            data: orderChartBar,
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)', 
                'rgba(54, 162, 235, 0.2)',  
                'rgba(255, 206, 86, 0.2)',  
                'rgba(75, 192, 192, 0.2)', 
                'rgba(153, 102, 255, 0.2)', 
                'rgba(255, 159, 64, 0.2)',  
                'rgba(244, 67, 54, 0.2)'   
            ],
            borderColor: [
                'rgba(255,99,132,1)',       
                'rgba(54, 162, 235, 1)',   
                'rgba(255, 206, 86, 1)',   
                'rgba(75, 192, 192, 1)',    
                'rgba(153, 102, 255, 1)',   
                'rgba(255, 159, 64, 1)',   
                'rgba(244, 67, 54, 1)'   
            ],
            borderWidth: 1,
            fill: false
        }]
    };

    var options = {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        },
        legend: {
            display: false
        },
        elements: {
            point: {
                radius: 0
            }
        }
    };

    if ($("#barChart").length) {
        var barChartCanvas = $("#barChart").get(0).getContext("2d");
        var barChart = new Chart(barChartCanvas, {
            type: 'bar',
            data: data,
            options: options
        });
    }
});

function getLast7Days() {
    var labels = [];
    var currentDate = new Date();

    for (var i = 6; i >= 0; i--) {
        var pastDate = new Date();
        pastDate.setDate(currentDate.getDate() - i);

        var day = pastDate.getDate();
        var month = pastDate.getMonth() + 1;
        labels.push('Day: ' + day + '/' + month);
    }

    return labels;
}

// 3 Line
const ctx = document.getElementById('myChart').getContext('2d');

const myChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: getLast7Days(),
        datasets: [
            {
                label: 'Revenue',
                data: orderChartLine,
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                fill: false,
                tension: 0.3
            },
            {
                label: 'Product Sold',
                data: productInOneDay,
                borderColor: 'rgba(54, 162, 235, 1)',
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                fill: false,
                tension: 0.3
            },
            {
                label: 'Orders',
                data: orderInOneDay,
                borderColor: 'rgba(255, 99, 132, 1)',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                fill: false,
                tension: 0.3
            }
        ]
    },
    options: {
        responsive: true,
        scales: {
            x: {
                title: {
                    display: true,
                    text: 'Day'
                }
            },
            y: {
                title: {
                    display: true,
                    text: 'Value'
                },
                beginAtZero: true
            }
        },
        plugins: {
            legend: {
                position: 'top'
            },
            tooltip: {
                mode: 'index',
                intersect: false
            }
        }
    }
});

//Pie Chart
$(function () {
    'use strict';

    var baseColors = [
        'rgba(255, 99, 132, 0.5)',
        'rgba(54, 162, 235, 0.5)',
        'rgba(255, 206, 86, 0.5)',
        'rgba(75, 192, 192, 0.5)',
        'rgba(153, 102, 255, 0.5)',
        'rgba(255, 159, 64, 0.5)',
        'rgba(255, 99, 71, 0.5)',
        'rgba(100, 149, 237, 0.5)',
        'rgba(144, 238, 144, 0.5)',
        'rgba(238, 130, 238, 0.5)'
    ];

    var borderColors = [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)',
        'rgba(255, 99, 71, 1)',
        'rgba(100, 149, 237, 1)',
        'rgba(144, 238, 144, 1)',
        'rgba(238, 130, 238, 1)'
    ];

    var doughnutPieData = {
        datasets: [{
            data: productInCategory,
            backgroundColor: baseColors.slice(0, categoryName.length),
            borderColor: borderColors.slice(0, categoryName.length)
        }],
        labels: categoryName
    };

    var doughnutPieOptions = {
        responsive: true,
        animation: {
            animateScale: true,
            animateRotate: true
        }
    };

    if ($("#pieChart").length) {
        var pieChartCanvas = $("#pieChart").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas, {
            type: 'pie',
            data: doughnutPieData,
            options: doughnutPieOptions
        });
    }
});
