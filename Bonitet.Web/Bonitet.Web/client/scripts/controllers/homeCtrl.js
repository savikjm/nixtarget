/**
 * Created by NixDev on 9/27/2016.
 */
app.controller('HomeCtrl', function ($scope, $rootScope, $http, FileSaver, Blob) {
    $scope.data = [];
    var reader = new FileReader();
    $scope.keys = [];
    $scope.showTable = false;
    $scope.json_object = null;

    $scope.load = function () {
					
        var fileInput = document.getElementById('fileInput');
        var fileDisplayArea = document.getElementById('fileDisplayArea');

        fileInput.addEventListener('change', function (e) {
		
            $scope.data = [];
            $scope.keys = [];
            $scope.json_object = null;
            $scope.showTable = false;
            var file = fileInput.files[0];
            var textType = 'xslx';
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                var workbook = XLSX.read(data, { type: 'binary', cellFormula: true });
                get_header_row(workbook.Sheets.Content);
                workbook.SheetNames.forEach(function (sheetName) {
                    var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                    $scope.json_object = JSON.stringify(XL_row_object);
                    var el = [];
                    angular.forEach(XL_row_object, function (row) {
                        el = [];
                        for (var i = 0; i < $scope.keys.length; i++) {
                            if (row[$scope.keys[i]] !== undefined)
                                el.push(parseFloat(row[$scope.keys[i]]));
                            else {
                                el.push(null);
                            }
                        }
                        $scope.data.push(el);
                    });
                })
            };
            reader.readAsBinaryString(file);
			$scope.safeApply();
        });

    };

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    var exportFile = function (workbook) {
        var wopts = { bookType: 'xlsx', bookSST: false, type: 'binary' };
        var wbout = XLSX.write(workbook, wopts);
        FileSaver.saveAs(new Blob([s2ab(wbout)], { type: "" }), "test.xlsx")
    };


    function get_header_row(sheet) {
        var headers = [];
        var range = XLSX.utils.decode_range(sheet['!ref']);
        var C, R = range.s.r;
        for (C = range.s.c; C <= range.e.c; ++C) {
            var cell = sheet[XLSX.utils.encode_cell({ c: C, r: R })]
            var hdr = "UNKNOWN " + C;
            if (cell && cell.t) hdr = XLSX.utils.format_cell(cell);
            $scope.keys.push(hdr);
        }
    }

    $scope.getElement = function (index) {
        return $scope.data[index];
    };

    $scope.read = function () {
        if ($scope.data.length > 0)
            $scope.showTable = true;
    };

    $scope.upload = function () {
        if ($scope.data.length > 0) {
			var type = 2;
			if($scope.keys.length > 20){
				type = 1;
			}
		    $.ajax({
				type: 'POST',
				url: "http://localhost:9998/DocumentHelper.ashx",
				data: "import_type=" + type + "&import_from_file=" + $scope.json_object,
				dataType: 'text',
				success: function (res) {
					if(res == "Error"){
						$scope.ErrMSsg = true;
					}else{
						$scope.SuccessMsg = true;
					}
					$scope.resetAll();
				},
				error: function (a, b, c) {
					$scope.ErrMSsg = true;
					$scope.resetAll();
				}
			});
        }

    };
	
	$scope.resetAll = function(){
		$('#fileInput').trigger('reset');
		$scope.data = [];
		$scope.keys = [];
		$scope.json_object = null;
		$scope.showTable = false;
		$scope.safeApply();
	}
	
	

	$scope.safeApply = function (fn) {
	  if ($scope.$root && !$scope.$root.$$phase) {
		var phase = this.$root.$$phase;

		if (phase == '$apply' || phase == '$digest') {
		  if (fn && (typeof (fn) === 'function')) {
			fn();
		  }
		} else {
		  this.$apply(fn);
		}
	  }
	};

});