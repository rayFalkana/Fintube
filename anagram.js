const anagramChecking = (word1,word2)=>{
    let firstWord = {}
    let secondWord = {}

    if (word1.length != word2.length){
        return false;
    }

    for(let word in word1){
        if(word in firstWord){
            firstWord[word] +=1
            continue
        }
        
        firstWord[word] = 1
    }

    for(let word in word2){
        if(word in secondWord){
            secondWord[word]+=1
            continue
        }

        secondWord[word] = 1
    }

    for (let item in secondWord){
        if(!(item in firstWord)){
            return false
        }else if(firstWord[item] !== secondWord[item]){
            return false
        }
    }
    return true
}