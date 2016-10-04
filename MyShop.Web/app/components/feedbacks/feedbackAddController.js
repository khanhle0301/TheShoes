(function (app) {
    app.controller('feedbackAddController', feedbackAddController);

    feedbackAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function feedbackAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.feedback = {
            CreatedDate: new Date(),
            Status: true
        }
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.AddFeedback = AddFeedback;
        function AddFeedback() {           
            apiService.post('api/feedback/create', $scope.feedback,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                    $state.go('feedbacks');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }       
    }

})(angular.module('myshop.feedbacks'));