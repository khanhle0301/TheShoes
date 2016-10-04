(function (app) {
    app.controller('slideEditController', slideEditController);

    slideEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function slideEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.slide = {};       
        $scope.UpdateSlide = UpdateSlide;             

        function loadSlideDetail() {
            apiService.get('api/slide/getbyid/' + $stateParams.id, null, function (result) {
                $scope.slide = result.data;           
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateSlide() {            
            apiService.put('api/slide/update', $scope.slide,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('slides');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
       
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.slide.Image = fileUrl;
                })
            }
            finder.popup();
        }
       
        loadSlideDetail();
    }

})(angular.module('myshop.slides'));