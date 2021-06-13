
import history from './History';
import isJson from './IsJson';
import { isUserAuthenticated } from './User'

export function post(url, data, headers) {
    return httpFetch(url, 'POST', data, headers);
}

export function get(url, data, headers, options) {
    return httpFetch(url, 'GET', data, headers, options);
}

export function put(url, data, headers) {
    return httpFetch(url, 'PUT', data, headers);
}

//the name delete gives problems, so I added X to the function name
export function deleteX(url, data, headers) {
    return httpFetch(url, 'DELETE', data, headers);
}

function httpFetch(url, method, data, headers, options) {
    var fetchOptionsObj;
    if (options)
        fetchOptionsObj = options;
    else
        fetchOptionsObj = {};

    // console.log("url: " + url + ' ' + "options: " + options);
    return new Promise((resolve, reject) => {
        fetchOptionsObj.method = method;
        fetchOptionsObj.cache = "no-store";
        fetchOptionsObj.headers = {
            'Content-Type': 'application/json',
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Credentials": "true",
            ...authHeader()
        };

        if (data && (method === "POST" || method === "PUT" || method === "DELETE"))
            fetchOptionsObj["body"] = JSON.stringify(data)
        fetch(url, fetchOptionsObj)
            .then(response => {
                if (options) console.log("than " + url + options.aborted );
             
                if (!response || !response.body || !isJson(response) || !isJson(response.body))
                    resolve({});
                else if (response.statusText === "No Content")
                    resolve(response.statusText);
                else if (response.status === 401) {
                    //console.log("401");
                    reject("Not authenticate 401 error");
                    if (window.location && window.location.pathname == "/login")
                        return;
                    history.push("/login");
                    window.location.reload();
                }
                else {
                    // console.log(url);
                    //console.log(response.status);
                    try {
                        console.log(response);
                        var s = response.json();
                        resolve(s);
                    }
                    catch( ex){
                        console.log("eror parsin response:(");
                        console.log(response);
                    }
                }
            })
            .catch(error => {
                // console.log("catch " + url + ' ' + options);
                // console.log(error);
                reject(error);
            });
        //}
    })

};

function authHeader() {
    if (!isUserAuthenticated) {
        history.push("/login");
        window.location.reload();
    }

    let token = localStorage.getItem("securityToken");
    if (token) {
        return { 'Authorization': 'Bearer ' + token };
    } else {
        return {};
    }
}


