
const containDuplicates = (nums) =>{
    let arr = {}

    for(let item of nums){
        if (item in arr) {
            return true;
        } else {
            arr[item] = true;
        }
    }
    return false;
}

console.log(containDuplicates(1,1,2,3,3,4,5))