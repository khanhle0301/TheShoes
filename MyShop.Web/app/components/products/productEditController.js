(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function productEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.product = {};
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.UpdateProduct = UpdateProduct;
        $scope.moreImages = [];
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        function loadProductDetail() {
            apiService.get('api/product/getbyid/' + $stateParams.id, null, function (result) {
                $scope.product = result.data;
                $scope.moreImages = JSON.parse($scope.product.MoreImages);
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages)
            apiService.put('/api/product/update', $scope.product, addSuccessed, addFailed);           
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.product.Name + ' đã được cập nhật.');
            $state.go('products');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        function loadProductCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        function loadProvider() {
            apiService.get('api/provider/getallparents', null, function (result) {
                $scope.providers = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.product.Image = fileUrl;
                })
            }
            finder.popup();
        }

        $scope.ChooseImage2 = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.product.Image2 = fileUrl;
                })
            }
            finder.popup();
        }
        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                })

            }
            finder.popup();
        }


        $scope.deleteItem = function (index) {
            $scope.moreImages.splice(index, 1);
        }

        function loadMaterials() {
            apiService.get('/api/material/getlistall',
                null,
                function (response) {
                    $scope.materials = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách chất liệu.');
                });

        }

        function loadColors() {
            apiService.get('/api/color/getlistall',
                null,
                function (response) {
                    $scope.colors = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách màu sắc.');
                });

        }

        function loadSizes() {
            apiService.get('/api/size/getlistall',
                null,
                function (response) {
                    $scope.sizes = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách size.');
                });

        }

        function loadHeights() {
            apiService.get('/api/height/getlistall',
                null,
                function (response) {
                    $scope.heights = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách chiều cao.');
                });

        }

        function loadTypes() {
            apiService.get('/api/type/getlistall',
                null,
                function (response) {
                    $scope.types = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách loại giầy.');
                });

        }

        function loadHeels() {
            apiService.get('/api/heel/getlistall',
                null,
                function (response) {
                    $scope.heels = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách gót giầy.');
                });

        }

        loadHeels();

        loadTypes();

        loadHeights();

        loadSizes();

        loadColors();

        loadMaterials();

        loadProvider();
        loadProductCategory();
        loadProductDetail();       
    }

})(angular.module('myshop.products'));