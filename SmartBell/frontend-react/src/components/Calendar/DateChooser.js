import React from 'react'
import {useState,useEffect} from 'react'

import {RangeDatePicker} from "react-google-flight-datepicker";
import "react-google-flight-datepicker/dist/main.css";
import DDMenu from '../Button/DDMenu';
import axios from "../../axios";


const DateChooser = ({onAdd}) => {
    const [startDate, setStartDate]=useState(new Date());
    const [endDate, setEndDate]=useState(new Date());
    
    const [sound,setSound]=useState('');
    const [radio, setRadio]=useState('');
    
    const [template,setTemplate]=useState([]);
    const ringOptions = ['Tízperces csengetési rend', 'Tizenöt perces csengetési rend', 'Rövidített órák'];

    const [files,setFiles]=useState([]);
    const [chosenFile,setChosenFile] = useState('');
    
    const ringOption = ringOptions[0];
    const soundOptions = ['Alap csengőhang'];
    const soundOption = soundOptions[0];
    const ttrOptions = ['Alap szöveg', 'ünnepi szöveg', 'covid tájékoztató'];
    const ttrOption = ttrOptions[0];

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
            .get(`/Template`)
            .then((response) => {
                const res = response.data;
                setTemplate(res);
                console.log(res);
            })
            .catch((error) => {
                console.log(error);
            });
      }, []);

    const onSubmit = (e)=>{
        e.preventDefault()
    
        if(startDate===endDate){
            alert('Kérlek add meg a kívánt időtartamot')
            return
        }
    
        console.log(startDate);
        console.log(endDate);
        //onAdd({startDate, endDate, template, sound, radio})
        //setStartDate(new Date())
        //setEndDate(new Date())
        setTemplate('Alapértelmezett')
        setSound('Alapértelmezett')
        setRadio('Alapértelmezett')
    }

    return (
        <form className='dpcontainer'>
            <h1>Csengetési rend gyorsbeállító</h1><br/>
            <p>Mettől meddig tartson a csengetési rend?</p><br/>
            <RangeDatePicker
                startDate={new Date()}
                endDate={new Date()}
            /><br/>
            <p>Válasszd ki a csengetések típusát:</p><br/>
            <DDMenu props={ringOptions} first={ringOption}/>
            <br/>
            <p>Válassz csengőhangot:</p><br/>
            <DDMenu onChange={e => setChosenFile(e.target.value)} props={files} first={files[0]} />
            <br/>
            <div><br/><br/><br/><br/><br/><br/></div>
            <input type='submit' className='btn btn-block' value='Csengetési rend hozzáadása'/>
        </form>
    )
}

export default DateChooser
