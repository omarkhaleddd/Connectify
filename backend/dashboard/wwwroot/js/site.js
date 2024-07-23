const data = {
    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
    datasets: [{
        label: 'Users Over Time',
        data: [12, 19, 3, 5, 2, 3, 7],
        borderColor: '#0d6efd',
        backgroundColor: 'rgba(0, 123, 255, 0.5)', 
    }]
};

let delayed;

window.createChart = () => {
    $(document).ready(function() {
        var ctx = $('#myChart')[0].getContext('2d');
        var ctx1 = $('#myChart1')[0].getContext('2d');
        
        var myChart = new Chart(ctx, {
            type: 'line',
            data: data,
            options: {
                animation: {
                    onComplete: () => {
                        delayed = true;
                    },
                    delay: (context) => {
                        let delay = 0;
                        if (context.type === 'data' && context.mode === 'default' && !delayed) {
                            delay = context.dataIndex * 300 + context.datasetIndex * 100;
                        }
                        return delay;
                    },
                },
                scales: {
                    x: {
                        beginAtZero: true
                    },
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });

        var myChart1 = new Chart(ctx1, {
            type: 'bar',
            data: data,
            options: {
                animation: {
                    onComplete: () => {
                        delayed = true;
                    },
                    delay: (context) => {
                        let delay = 0;
                        if (context.type === 'data' && context.mode === 'default' && !delayed) {
                            delay = context.dataIndex * 300 + context.datasetIndex * 100;
                        }
                        return delay;
                    },
                },
                scales: {
                    x: {
                        beginAtZero: true
                    },
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    });
};

function initializeWorldMap() {
    $('#world-map').vectorMap({
        map: 'world_mill',
        backgroundColor: '#a4b0be',
        regionStyle: {
            initial: {
                fill: '#1C8CC9',
            },
        },
        onRegionClick: function (event, code) {
            alert(`You clicked on region: ${code}`);
        },
    });
}
