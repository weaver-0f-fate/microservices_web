import React, { useEffect, useRef } from 'react';
import FullCalendar from "@fullcalendar/react";
import { Box } from "@mui/material"
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import rrulePlugin from '@fullcalendar/rrule'
import EventDialog from './components/eventDialog';
import './styles/eventsCalendar.css';
import { EventResponse, useGetEvents } from './fetch/useGetEvents';
import DisplayFilters from './components/filters/displayFilters';
import CreateEventDialog from './components/createEventDialog';
import Search from './components/search';
import { getLocalDate } from '../../shared/utilities/dateFunctions';
import { setEvents } from './store/eventsSlice';
import { useDispatch, useSelector } from 'react-redux';
import { Filters } from './store/filtersSlice';
import { setSelectedEventUuid } from './store/selectedEventSlice';

export interface EventBaseInfo {
    title: string;
    rrule?: string,
    date: Date;
    category: string,
    place: string,
    uuid: string;
}

const EventsCalendar = () => {
    const getEvents = useGetEvents();
    const eventDialogRef = useRef<any>();
    const createEventDialogRef = useRef<any>();
    const events = useSelector((state: any) => state.events);
    const filters = useSelector((state: any) => state.filters as Filters);
    const dispatch = useDispatch();

    useEffect(() => {
        getEvents(filters.category, filters.place, filters.time)
            .then(result => {
                const mappedResults = result.map((event: EventResponse) => {
                    return {
                        title: event.title,
                        rrule: event.rrule ?? undefined,
                        date: getLocalDate(event.date),
                        category: event.category,
                        place: event.place,
                        uuid: event.uuid
                    }
                }) as EventBaseInfo[];

                dispatch(setEvents(mappedResults));
            })

    }, [filters.category, filters.place, filters.time])

    const handleEventClick = (event: any) => {
        console.log(event.event.extendedProps.uuid)
        dispatch(setSelectedEventUuid(event.event.extendedProps.uuid));
        eventDialogRef.current.openDialog();
    }

    return (
        <Box display="flex" flexDirection="column">
            <Search openDialog={() => eventDialogRef.current.openDialog()} />
            <Box display="flex" flexDirection="row" >
                <DisplayFilters />
                <Box p={2} width={'100%'}>
                    <FullCalendar
                        plugins={[dayGridPlugin, interactionPlugin, timeGridPlugin, rrulePlugin]}
                        eventClick={handleEventClick}
                        initialView="dayGridMonth"
                        events={events}
                        eventTimeFormat={{
                            hour: '2-digit',
                            minute: '2-digit',
                            hour12: false
                        }}
                        headerToolbar={{
                            left: 'prev,next today create',
                            center: 'title',
                            right: 'dayGridMonth,timeGridWeek,timeGridDay',
                        }}
                        customButtons={{
                            create: {
                                text: 'Create',
                                click: () => createEventDialogRef.current.openDialog()
                            }
                        }}
                        />
                    <EventDialog ref={eventDialogRef}/>
                    <CreateEventDialog ref={createEventDialogRef} />
                </Box>
            </Box>
        </Box>
    )
}

export default EventsCalendar;