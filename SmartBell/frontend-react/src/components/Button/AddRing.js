import React from 'react'
import {useState, useEffect} from 'react'
import dateFormatter from 'dateformat';
import 'rc-time-picker/assets/index.css';
import DDMenu from './DDMenu';
import TPicker from '../Calendar/TPicker'
import axios from "../../axios";
import '../../index.css'

const AddRing = ({date}) => {
    const ringOptions = ['Normál', 'Csengetések némítása','Iskolarádió','Speciális csengetés'];
    const radioOptions = ['Alap rádióműsor', 'műsor2', 'műsor3'];

    const [ringType, setRingType]=useState(ringOptions[0]);
    
    const [startTime,setStartTime] = useState(new Date());
    const [endTime,setEndTime] = useState(new Date());
    const [description, setDescription] = useState('');
    const [length, setLength] = useState(0);
    const file = 'emptyFile'

    const [chosenFile,setChosenFile] = useState('');
    const [files,setFiles] = useState([]);
    const [radioSeqs, setRadioSeqs] = useState([]);

    const chosenDate = date;

    function onStartChange(value) {
        //const currenMonths = startTime.getMonth();
        //const currentDay = startTime.getDay();
        setStartTime(dateFormatter(value, "yyyy-mm-dd'T'HH:MM:ss"));
        //startTime.setMonth(currenMonths);
        //startTime.setDay(currentDay);
        console.log(value);
    }

    function onEndChange(value) {
        //const currenMonths = endTime.getMonth();
        //const currentDay = endTime.getDay();
        setEndTime(dateFormatter(value, "yyyy-mm-dd'T'HH:MM:ss"));
        //endTime.setMonth(currenMonths);
        //endTime.setDay(currentDay);
        console.log(value);
    }

    useEffect(() => {
        axios
            .get(`/File/GetAllFiles/`)
            .then((response) => {
                const res = response.data;
                setFiles(res);
                console.log(res);
            })
            .catch((error) => {
                console.log(error);
            });
        axios
            .get(`/Bellring/GetAllSequencedBellRings`)
            .then((response) => {
                const res = response.data;
                setRadioSeqs(res);
                console.log(res);
            })
            .catch((error) => {
                console.log(error);
            });
      }, [ringType]);


     //add ring
    const addRing = () =>{
        
        if(ringType==='Normál')
        {
            console.log(startTime);
            console.log(endTime);
            console.log(description);
    
            const data = {
                "startBellRing": {
                    "description": description,
                    "bellRingTime": startTime,
                    "intervalSeconds": length
                  },
                  "endBellring": {
                    "description": description,
                    "bellRingTime": endTime,
                    "intervalSeconds": length
                  },
                "startFilename": "",
                "endFilename": ""
            }
    
            axios.post(`/Bellring/InsertLessonBellrings`, data)
            .catch((error) => {
                console.log(error);
            });
        }

        if(ringType==='Csengetések némítása'){
            axios.post(`/Holiday/DeleteBellRingsInRange/${startTime}&${endTime}`)
            .catch((error) => {
                console.log(error);
            });
        }

        if(ringType==='Speciális csengetés'){
            
            const data = {
                "bellRing": {
                  "id": "",
                  "description": description,
                  "bellRingTime": startTime,
                  "intervalSeconds": length,
                },
                "fileName": chosenFile
              }

            axios.post(`/Bellring/InsertSpecialBellring`, data)
            .catch((error) => {
                console.log(error);
            });
        }
    }

    useEffect(() => {
        console.log(ringType)
    }, [ringType])
    
    const onSubmit = (e)=>{
        e.preventDefault()
        addRing();
        //window.location.reload(false);
    }

    return (
        <form className='container' onSubmit={onSubmit}>
            <div className='form-control'>
                <label>Szünet típusa </label>
                <select onChange={(e)=> setRingType(e.target.value)}>
                    <option value={ringOptions[0]}>
                        Normál
                    </option>
                    <option value={ringOptions[1]}>
                        Csengetések némítása
                    </option>
                    <option value={ringOptions[2]}>
                        Iskolarádió
                    </option>
                    <option value={ringOptions[3]}>
                        Speciális csengetés
                    </option>
                </select>
            </div>

            <div className='form-control'>
                <label>Szünet kezdete</label>
                <TPicker onChange={onStartChange}/>
            </div>

            {
                ringType=='Csengetések némítása' && 
                <div>
                    <div className='form-control'>
                        <label>Szünet vége</label>
                        <TPicker onChange={onEndChange}/>
                    </div>
                </div>
            }
            {
                ringType=='Normál' && 
                <div>
                    <div className='form-control'>
                        <label>Szünet vége</label>
                        <TPicker onChange={onEndChange}/>
                    </div>
                    <div className='form-control'>
                        <p>Csengetés leírása:</p><br/>
                        <input placeholder='alapértelmezett' type='text' onChange={e => setDescription(e.target.value)}/><br/>
                    </div>
                    <div className='form-control'>
                        <p>Add meg a csengetések hosszát másodpercben:</p><br/>
                        <input placeholder='automatikus' type='number' onChange={e => setLength(e.target.value)}/><br/>
                    </div>
                </div>
            }
            {
                ringType=='Iskolarádió' && 
                <div>
                    <div className='form-control'>
                        <p>Csengetés leírása:</p><br/>
                        <input placeholder='alapértelmezett' type='text' onChange={e => setDescription(e.target.value)}/><br/>
                    </div>
                    <div className='form-control'>
                        <label>Válassz rádióműsort</label>
                        <DDMenu props={/*radioSeqs.description*/radioOptions} first={radioOptions[0]} />
                    </div>
                </div>
            }
            {
                ringType=='Speciális csengetés' && 
                <div>
                    <div className='form-control'>
                        <p>Csengetés leírása:</p><br/>
                        <input placeholder='alapértelmezett' type='text' onChange={e => setDescription(e.target.value)}/><br/>
                    </div>
                    <div className='form-control'>
                        <label>Válassz lejátszandó fájlt</label>
                        <DDMenu onChange={e => setChosenFile(e.target.value)} props={files} first={files[0]} />
                    </div>
                </div>
            }
            
            <input type='submit' className='btn btn-block' value='Csengetés hozzáadása'/>
        </form>
    )
}

export default AddRing
