(function (app) {
    app.controller('feedbackEditController', feedbackEditController);

    feedbackEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function feedbackEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.feedback = {};
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.UpdateFeedback = UpdateFeedback;

        function loadFeedbackDetail() {
            apiService.get('api/feedback/getbyid/' + $stateParams.id, null, function (result) {
                $scope.feedback = result.data;            
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateFeedback() {          
            apiService.put('api/feedback/update', $scope.feedback,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('feedbacks');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }        
        loadFeedbackDetail();
    }

})(angular.module('myshop.feedbacks'));