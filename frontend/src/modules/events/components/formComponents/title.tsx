import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';
import { setEventTitle } from "../../store/eventsSlice";
import { setSelectedEventTitle } from "../../store/selectedEventSlice";

const TitleField = () => {
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const dispatch = useDispatch();
    const disabled = event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(event.title)
            inputRef.current.value = event.title;
    }, [event])

    const handleTitleChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(event.uuid, '/title', value)
            .then(() => {
                dispatch(setSelectedEventTitle(value));
                dispatch(setEventTitle({uuid: event.uuid, title: value}));
            })
            .catch(err => console.log(err));
        }
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="title-input">Title</InputLabel>
            <TextField
                id="title-input"
                fullWidth
                defaultValue={''}
                inputRef={inputRef}
                onChange={handleTitleChange}
                disabled={disabled}
            />
        </Grid>
    )
}

export default TitleField;