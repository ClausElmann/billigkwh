var eventListener = function(e) {
          if (e.data.type && e.data.type == "HeightChanged") {
			var iFrame = document.getElementById("iFrame-module");
				if(iFrame) {
					iFrame.height = e.data.height;
					}    
            }
        }
window.removeEventListener("message", eventListener);
window.addEventListener("message", eventListener);